using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CVA.AddOn.Common.Controllers
{
    public class SqlController
    {
        private static SqlConnection Connection = new SqlConnection();
        private SqlDataAdapter DataAdapter = new SqlDataAdapter();
        private SqlDataReader DataReader;
        private SqlCommand Command;
        private static SqlTransaction Transaction;

        public string TableName { get; set; }
        public object Model { get; set; }

        private string ConnectionString;

        public SqlController()
        {
            this.ConnectionString = GetConnectionString(CVAApp.ServerName, CVAApp.DatabaseName, CVAApp.DBUserName, CVAApp.DBPassword);
        }

        public SqlController(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SqlController(string serverName, string dataBaseName, string userName, string userPassword)
        {
            this.ConnectionString = GetConnectionString(serverName, dataBaseName, userName, userPassword);
        }

        #region GetNextCode
        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <returns>Código</returns>
        public static string GetNextCode(string tableName)
        {
            return GetNextCode(tableName, "Code", String.Empty);
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
            string sql = String.Format(" SELECT ISNULL(MAX(CAST({0} AS BIGINT)), 0) + 1 FROM [{1}] ", fieldName, tableName);

            if (!String.IsNullOrEmpty(where))
            {
                sql += String.Format(" WHERE {0} ", where);
            }

            SqlController controller = new SqlController();
            object code = controller.ExecuteScalar(sql);

            if (code != null)
            {
                return Convert.ToInt32(code).ToString();
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion GetNextCode

        #region CreateModel
        /// <summary>
        /// Salva o model no banco de dados
        /// </summary>
        public void CreateModel()
        {
            try
            {
                ModelControllerAttribute modelController;

                string sqlColumns = String.Empty;
                string sqlValues = String.Empty;

                List<Type> typesList = new List<Type>();
                List<object> valuesList = new List<object>();


                //Dictionary<Type, object> values = new Dictionary<Type, object>();
                //List<object> values = new List<object>();
                // Percorre as propriedades do Model
                foreach (PropertyInfo property in Model.GetType().GetProperties())
                {
                    try
                    {
                        // Busca os Custom Attributes
                        foreach (Attribute attribute in property.GetCustomAttributes(true))
                        {
                            modelController = attribute as ModelControllerAttribute;

                            if (modelController != null)
                            {
                                // Se não for DataBaseField não seta nas properties
                                if (!modelController.DataBaseFieldYN)
                                {
                                    break;
                                }

                                typesList.Add(property.PropertyType);
                                valuesList.Add(property.GetValue(Model, null));

                                if (String.IsNullOrEmpty(modelController.ColumnName))
                                {
                                    modelController.ColumnName = property.Name;
                                }

                                sqlColumns += String.Format(", {0}", modelController.ColumnName);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e));
                    }
                }
                if (String.IsNullOrEmpty(sqlColumns))
                {
                    throw new Exception("Nenhuma coluna informada. Informe a propriedade ColumnName no Model");
                }

                for (int i = 0; i < typesList.Count; i++)
                {
                    if (valuesList[i] == null || valuesList[i] == DBNull.Value)
                    {
                        sqlValues += ", NULL";
                    }
                    else if (typesList[i] == typeof(string) || typesList[i] == typeof(String) || typesList[i] == typeof(char))
                    {
                        sqlValues += String.Format(", '{0}'", valuesList[i].ToString().Replace("'", "''"));
                    }
                    else if (typesList[i] == typeof(DateTime) || typesList[i] == typeof(Nullable<DateTime>))
                    {
                        sqlValues += String.Format(", CONVERT(DATETIME, '{0}') ", ((DateTime)valuesList[i]).ToString("yyyy-MM-ddTHH:mm:ss"));
                    }
                    else
                    {
                        sqlValues += String.Format(", '{0}' ", valuesList[i].ToString().Replace(",", "."));
                    }
                }

                string sql = String.Format(" INSERT INTO [{0}] ({1}) VALUES ({2}) ", TableName, sqlColumns.Substring(2), sqlValues.Substring(2));

                this.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region UpdateModel
        public void UpdateModel()
        {
            try
            {
                ModelControllerAttribute modelController;

                string sqlWhere = String.Empty;
                string sqlValues = String.Empty;
                object value;

                Dictionary<Type, object> values = new Dictionary<Type, object>();
                //List<object> values = new List<object>();
                // Percorre as propriedades do Model
                foreach (PropertyInfo property in Model.GetType().GetProperties())
                {
                    try
                    {
                        // Busca os Custom Attributes
                        foreach (Attribute attribute in property.GetCustomAttributes(true))
                        {
                            modelController = attribute as ModelControllerAttribute;

                            if (modelController != null)
                            {
                                // Se não for DataBaseField não seta nas properties
                                if (!modelController.DataBaseFieldYN)
                                {
                                    break;
                                }
                                if (String.IsNullOrEmpty(modelController.ColumnName))
                                {
                                    modelController.ColumnName = property.Name;
                                }

                                value = property.GetValue(Model, null);
                                if (modelController.IsPK || property.Name == "Code")
                                {
                                    if (value == null)
                                    {
                                        sqlWhere += String.Format(" AND {0} = NULL", modelController.ColumnName);
                                    }
                                    else if (property.PropertyType == typeof(string))
                                    {
                                        sqlWhere += String.Format(" AND {0} = '{1}'", modelController.ColumnName, value);
                                    }
                                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        sqlWhere += String.Format(" AND {0} = CONVERT(DATETIME, '{1}')", modelController.ColumnName, Convert.ToDateTime(value).ToString("yyyyMMdd"));
                                    }
                                    else
                                    {
                                        sqlWhere += String.Format(" AND {0} = {1}", modelController.ColumnName, value.ToString().Replace(",", "."));
                                    }
                                }
                                else
                                {
                                    if (value == null)
                                    {
                                        sqlValues += String.Format(", {0} = NULL", modelController.ColumnName);
                                    }
                                    else if (property.PropertyType == typeof(string))
                                    {
                                        sqlValues += String.Format(", {0} = '{1}'", modelController.ColumnName, value.ToString().Replace("'", "''"));
                                    }
                                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        sqlValues += String.Format(", {0} = CONVERT(DATETIME, '{1}')", modelController.ColumnName, Convert.ToDateTime(value).ToString("yyyyMMdd"));
                                    }
                                    else
                                    {
                                        sqlValues += String.Format(", {0} = {1}", modelController.ColumnName, value.ToString().Replace(",", "."));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e));
                    }
                }

                if (String.IsNullOrEmpty(sqlWhere))
                {
                    throw new Exception("Nenhuma coluna PK informada. Informe a propriedade IsPK no Model ou crie um campo chamado 'Code' (será utilizado como PK por default)");
                }
                if (String.IsNullOrEmpty(sqlValues))
                {
                    throw new Exception("Nenhuma coluna informada para atualizar. Informe a propriedade ColumnName no Model");
                }

                string sql = String.Format(" UPDATE [{0}] SET {1} WHERE {2} ", TableName, sqlValues.Substring(2), sqlWhere.Substring(4));
                this.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        public string GetConnectedServer()
        {
            return Connection.DataSource;
        }

        public void Connect()
        {
            if (Connection == null || Connection.ConnectionString != this.ConnectionString)
            {
                Connection = new SqlConnection();                
            }

            if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed)
            {
                try
                {
                    Connection.ConnectionString = this.ConnectionString;
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao conectar SQL: " + ex.Message);
                }
            }
        }

        public void Close()
        {
            if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Executing || Connection.State == ConnectionState.Fetching)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }

        public SqlDataReader ExecuteReader(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.Transaction = Transaction;
                this.DataReader = Command.ExecuteReader();
                return this.DataReader;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar SqlDataReader: " + ex.Message);
            }
        }

        public object ExecuteScalar(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.Transaction = Transaction;
                return this.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao executar ExecuteScalar: " + ex.Message);

            }
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.Transaction = Transaction;
                this.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteNonQuery: " + ex.Message);
            }
        }

        public DataTable FillDataTable(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.CommandType = CommandType.Text;
                this.Command.CommandText = sql;
                DataAdapter.SelectCommand = this.Command;

                DataTable dtb = new DataTable();

                DataAdapter.Fill(dtb);
                DataAdapter.Dispose();
                return dtb;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar FillDataTable: " + ex.Message);
            }
        }

        public void BeginTransaction()
        {
            this.Connect();
            Transaction = Connection.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (Transaction.Connection != null)
            {
                Transaction.Rollback();
            }
        }

        public void CommitTransaction()
        {
            if (Transaction.Connection != null)
            {
                Transaction.Commit();
            }
        }

        public static string GetConnectionString(string serverName, string dataBaseName, string userName, string userPassword)
        {
            string connectionString = String.Format(@" data source={0};initial catalog={1};persist security info=True;user id={2};password={3};",
                                                serverName,
                                                dataBaseName,
                                                userName,
                                                userPassword);
            return connectionString;
        }

        public T FillModel<T>(string sql)
        {
            List<T> modelList = this.FillModelList<T>(sql);
            if (modelList.Count > 0)
            {
                return modelList[0];
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public int GetRowCount(string sql)
        {
            int recordCount = 0;
            using (SqlDataReader dr = this.ExecuteReader(sql))
            {
                if (dr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    recordCount = dt.Rows.Count;
                }
            }

            return recordCount;
        }

        public List<string> FillStringList(string sql)
        {

            List<string> list = new List<string>();
            using (SqlDataReader dr = this.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        list.Add(dr.GetValue(0).ToString());
                    }
                    else
                    {
                        list.Add(String.Empty);
                    }
                }
            }
            return list;
        }

        public List<T> FillModelList<T>(string sql)
        {
            List<T> modelList = new List<T>();
            T model;
            ModelControllerAttribute modelController;
            try
            {
                using (SqlDataReader dr = this.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        // Cria nova instância do model
                        model = Activator.CreateInstance<T>();
                        // Seta os valores no model
                        foreach (PropertyInfo property in model.GetType().GetProperties())
                        {
                            try
                            {
                                // Busca os Custom Attributes
                                foreach (Attribute attribute in property.GetCustomAttributes(true))
                                {
                                    modelController = attribute as ModelControllerAttribute;
                                    if (modelController != null)
                                    {
                                        // Se propriedade "ColumnName" estiver vazia, pega o nome da propriedade
                                        if (String.IsNullOrEmpty(modelController.ColumnName))
                                        {
                                            modelController.ColumnName = property.Name;
                                        }
                                        if (!modelController.DataBaseFieldYN && !modelController.FillOnSelect)
                                        {
                                            break;
                                        }

                                        int index = dr.GetOrdinal(modelController.ColumnName);
                                        if (!dr.IsDBNull(index))
                                        {
                                            Type dbType = dr.GetFieldType(index);

                                            if (dbType == typeof(decimal) && property.PropertyType == typeof(double))
                                            {
                                                property.SetValue(model, Convert.ToDouble(dr.GetValue(index)), null);
                                            }
                                            else
                                            {
                                                property.SetValue(model, dr.GetValue(index), null);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e.Message));
                            }
                        }
                        modelList.Add(model);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return modelList;
        }

        public T FillModelAccordingToSql<T>(string sql)
        {
            List<T> modelList = this.FillModelListAccordingToSql<T>(sql);
            if (modelList.Count > 0)
            {
                return modelList[0];
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public List<T> FillModelListAccordingToSql<T>(string sql)
        {
            List<T> modelList = new List<T>();
            T model;
            using (SqlDataReader dr = this.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    // Cria nova instância do model
                    model = Activator.CreateInstance<T>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        PropertyInfo property = model.GetType().GetProperty(dr.GetName(i));
                        if (!dr.IsDBNull(i))
                        {
                            property.SetValue(model, dr.GetValue(i), null);
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
    }
}
