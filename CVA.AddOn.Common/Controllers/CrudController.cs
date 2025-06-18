using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using CVA.AddOn.Common.Enums;
using System.Runtime.InteropServices;
using CVA.AddOn.Common.Models;
using System.Globalization;
using SAPbobsCOM;

namespace CVA.AddOn.Common.Controllers
{
    public class CrudController
    {
        public string TableName { get; set; }
        public object Model { get; set; }
        public BoUTBTableType UserTableType { get; set; }

        public CrudController()
        {
            
        }

        public CrudController(string tableName)
        {
            TableName = tableName;
        }

        #region CRUD
        #region CreateUpdateModel
        /// <summary>
        /// Insere dados no banco
        /// </summary>
        /// <param name="model">Objeto do tipo Model</param>
        /// <param name="tableName">Nome da tabela</param>
        public void CreateModel()
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.UserTableType = UserTableType;
                controller.TableName = TableName;
                controller.Model = Model;
                controller.CreateModel();
            }
            else
            {
                SqlController controller = new SqlController();
                controller.TableName = TableName;
                controller.Model = Model;
                controller.CreateModel();
            }
        }

        /// <summary>
        /// Atualiza dados no banco
        /// </summary>
        /// <param name="model">Objeto do tipo Model</param>
        /// <param name="tableName">Nome da tabela</param>
        public void UpdateModel()
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.UserTableType = UserTableType;
                controller.TableName = TableName;
                controller.Model = Model;
                controller.UpdateModel();
            }
            else
            {
                SqlController controller = new SqlController();
                controller.TableName = TableName;
                controller.Model = Model;
                controller.UpdateModel();
            }
        }

        /// <summary>
        /// Atualiza dados no banco
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição WHRE</param>
        /// <param name="model">Model com os dados a serem atualizados</param>
        public void UpdateModel(string where)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.UserTableType = UserTableType;
                controller.TableName = TableName;
                controller.Model = Model;
                controller.UpdateModel(where);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion CreateUpdateModel

        public string RetrieveModelSql(Type modelType, string where, string orderBy, bool getValidValues)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.TableName = TableName;
                return controller.RetrieveModelSql(modelType, where, orderBy, getValidValues);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #region RetrieveModel
        /// <summary>
        /// Retorna Model preenchido de acordo com a condição WHERE
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição da consulta</param>
        /// <returns>Model</returns>
        public T RetrieveModel<T>(string where)
        {
            return this.RetrieveModel<T>(where, String.Empty);
        }

        /// <summary>
        /// Retorna Model preenchido de acordo com a condição WHERE
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição da consulta</param>
        /// <param name="orderBy">Ordenação</param>
        /// <returns>Model</returns>
        public T RetrieveModel<T>(string where, string orderBy)
        {
            List<T> modelList = this.RetrieveModelList<T>(where, orderBy);
            if (modelList.Count > 0)
                return modelList[0];
            else
                return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Retorna lista de Models de acordo com a condição WHERE
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição da consulta</param>
        /// <returns>ModelList</returns>
        public List<T> RetrieveModelList<T>(string where)
        {
            return this.RetrieveModelList<T>(String.Empty, String.Empty, where, String.Empty);
        }

        /// <summary>
        /// Retorna lista de Models de acordo com a condição WHERE
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição da consulta</param>
        /// <param name="orderBy">Ordenação</param>
        /// <returns>ModelList</returns>
        public List<T> RetrieveModelList<T>(string where, string orderBy)
        {
            return this.RetrieveModelList<T>(String.Empty, String.Empty, where, orderBy);
        }

        /// <summary>
        /// Retorna lista de Models de acordo com a condição WHERE
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="tableName">Nome da tabela</param>
        /// /// <param name="joinTable">Tabela </param>
        /// /// <param name="joinCondition">Nome da tabela</param>
        /// <param name="where">Condição da consulta</param>
        /// <param name="orderBy">Ordenação</param>
        /// <returns>ModelList</returns>
        public List<T> RetrieveModelList<T>(string joinTable, string joinCondition, string where, string orderBy)
        {
            StringBuilder sql = new StringBuilder();
            // Inicia o SELECT
            sql.Append(" SELECT ");

            Type modelType = typeof(T);
            ModelControllerAttribute modelController;

            string fields = String.Empty;
            string fieldTableName;
            // Percorre as propriedades do Model para montar o SELECT
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                // Busca os Custom Attributes
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    modelController = attribute as ModelControllerAttribute;
                    if (modelController == null)
                    {
                        continue;
                    }
                    // Se propriedade "ColumnName" estiver vazia, pega o nome da propriedade
                    if (String.IsNullOrEmpty(modelController.ColumnName))
                        modelController.ColumnName = property.Name;
                    if (modelController != null)
                    {
                        // Se não for DataBaseField não adiciona no select
                        if (!modelController.DataBaseFieldYN)
                        {
                            break;
                        }
                        fieldTableName = String.IsNullOrEmpty(modelController.TableName) ? TableName : modelController.TableName;
                        if (CVAApp.DatabaseType == DatabaseTypeEnum.HANA)
                        {
                            fields += String.Format(", {1} ", fieldTableName, modelController.ColumnName);
                        }
                        else
                        {
                            fields += String.Format(", [{0}].{1} AS {1} ", fieldTableName, modelController.ColumnName);
                        }
                    }
                    break;
                }
            }

            if (String.IsNullOrEmpty(fields))
            {
                throw new Exception("Nenhuma propriedade do tipo ModelController encontrada no Model");
            }

            // Campos a serem retornados
            sql.Append(fields.Substring(1));

            // TABELA
            if (CVAApp.DatabaseType == DatabaseTypeEnum.HANA)
            {
                sql.AppendFormat(" FROM [{0}] ", TableName);
            }
            else
            {
                sql.AppendFormat(" FROM [{0}] WITH(NOLOCK) ", TableName);
            }
            // INNER JOIN
            if (!String.IsNullOrEmpty(joinTable))
            {
                sql.AppendFormat(" INNER JOIN {0} ", joinTable);
                if (String.IsNullOrEmpty(joinCondition))
                {
                    joinCondition = " 1 = 1 ";
                }
                sql.AppendFormat(" ON {0} ", joinCondition);
            }

            // Condição WHERE
            if (!String.IsNullOrEmpty(where))
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }

            // Condição ORDER BY
            if (!String.IsNullOrEmpty(orderBy))
            {
                sql.AppendFormat(" ORDER BY {0} ", orderBy);
            }

            return FillModelList<T>(sql.ToString());
        }
        #endregion RetrieveModel

        #region Delete
        /// <summary>
        /// Deleta registro
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição WHERE</param>
        public void DeleteModel(string tableName, string where)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.DeleteModel(tableName, where);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteModelByCode(string tableName, string code)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.DeleteModelByCode(tableName, code);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion Delete
        #endregion CRUD

        #region Util
        #region GetNextCode
        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <returns>Código</returns>
        public static string GetNextCode(string tableName)
        {
            var t = GetNextCode(tableName, "Code", String.Empty);
            return t;
        }

        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns>Código</returns>
        public static string GetNextCode(string tableName, string fieldName)
        {
            return GetNextCode(tableName, fieldName, String.Empty);
        }

        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="fieldName">Nome do campo</param>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Where</param>
        /// <returns>Código</returns>
        public static string GetNextCode(string tableName, string fieldName, string where)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                return CrudB1Controller.GetNextCode(tableName, fieldName, where);
            }
            else
            {
                return SqlController.GetNextCode(tableName, fieldName, where);
            }
        }
        #endregion GetNextCode

        #region GetColumnValue
        /// <summary>
        /// Retorna valor da coluna de acordo com o select
        /// </summary>
        public static T GetColumnValue<T>(string sql)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                return CrudB1Controller.GetColumnValue<T>(sql);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        public List<string> FillStringList(string sql)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                return CrudB1Controller.FillStringList(sql);
            }
            else
            {
                SqlController controller = new SqlController();
                return controller.FillStringList(sql);
            }
        }

        #region FillModel
        /// <summary>
        /// Preenche model através do SQL
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="sql">Comando SQL</param>
        /// <returns>Lista de Model preenchido</returns>
        public T FillModel<T>(string sql)
        {
            List<T> modelList = this.FillModelList<T>(sql);
            if (modelList.Count > 0)
                return modelList[0];
            else
                return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Preenche a lista de model através do SQL
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="sql">Comando SQL</param>
        /// <returns>Lista de Model preenchido</returns>
        public List<T> FillModelList<T>(string sql)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.TableName = TableName;
                return controller.FillModel<T>(sql);
            }
            else
            {
                SqlController controller = new SqlController();
                controller.TableName = TableName;
                return controller.FillModelList<T>(sql);
            }
        }
        #endregion FillModel

        #region FillModelAccordingTOSql
        /// <summary>
        /// Preenche model através do SQL
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="sql">Comando SQL</param>
        /// <returns>Lista de Model preenchido</returns>
        public T FillModelAccordingToSql<T>(string sql)
        {
            List<T> modelList = this.FillModelListAccordingToSql<T>(sql);
            if (modelList.Count > 0)
                return modelList[0];
            else
                return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Preenche a lista de model através do SQL
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="sql">Comando SQL</param>
        /// <returns>Lista de Model preenchido</returns>
        public List<T> FillModelListAccordingToSql<T>(string sql)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.TableName = TableName;
                return controller.FillModelAccordingToSql<T>(sql);
            }
            else
            {
                SqlController controller = new SqlController();
                controller.TableName = TableName;
                return controller.FillModelListAccordingToSql<T>(sql);
            }
        }
        #endregion FillModel

        #region Exists
        /// <summary>
        /// Verifica se registro existe
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Condição WHERE</param>
        /// <returns>Código do registro</returns>
        public string Exists(string where)
        {
            return this.Exists("Code", where);
        }

        /// <summary>
        /// Retorna a quantidade de linhas de uma query
        /// </summary>
        /// <param name="sql">SELECT</param>
        /// <returns></returns>
        public int GetRowCount(string sql)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                return controller.GetRowCount(sql);
            }
            else
            {
                SqlController controller = new SqlController();
                return controller.GetRowCount(sql);
            }
        }

        /// <summary>
        /// Verifica se registro existe
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="returnColumn">Coluna a ser retornada</param>
        /// <param name="where">Condição WHERE</param>
        /// <returns>Código do registro</returns>
        public string Exists(string returnColumn, string where)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.TableName = TableName;
                return controller.Exists(returnColumn, where);
            }
            else
            {
                SqlController controller = new SqlController();
                controller.TableName = TableName;
                throw new NotImplementedException();
            }
        }
        #endregion Exists

        public static void ExecuteNonQuery(string sql)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller.ExecuteNonQuery(sql);
            }
            else
            {
                SqlController controller = new SqlController();
                controller.ExecuteNonQuery(sql);
            }
        }

        public static object ExecuteScalar(string sql, bool translate = true)
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                return CrudB1Controller.ExecuteScalar(sql, translate);
            }
            else
            {
                SqlController controller = new SqlController();
                return controller.ExecuteScalar(sql);
            }
        }
        #endregion Util

        #region Transaction
        public void BeginTransaction()
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.BeginTransaction();
            }
            else
            {
                SqlController controller = new SqlController();
                controller.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.CommitTransaction();
            }
            else
            {
                SqlController controller = new SqlController();
                controller.CommitTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.RollbackTransaction();
            }
            else
            {
                SqlController controller = new SqlController();
                controller.RollbackTransaction();
            }
        }

        public void InTransaction()
        {
            if (CVAApp.AppType == AppTypeEnum.SBO)
            {
                CrudB1Controller controller = new CrudB1Controller();
                controller.RollbackTransaction();
            }
            else
            {
                SqlController controller = new SqlController();
                controller.RollbackTransaction();
            }
        }
        #endregion
    }
}
