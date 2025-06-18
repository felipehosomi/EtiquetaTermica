using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CVA.AddOn.Common.Controllers
{
    public class GridController
    {
        public void FillTableFromModel<T>(List<T> modelList, ref DataTable table)
        {
            table.Rows.Add(modelList.Count);

            Type modelType = typeof(T);
            // Seta os valores no model
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                // Busca os Custom Attributes
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    int i = 0;
                    foreach (var item in modelList)
                    {
                        ModelControllerAttribute modelController = attribute as ModelControllerAttribute;
                        if (modelController != null)
                        {
                            if (!String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                table.SetValue(modelController.UIFieldName, i, property.GetValue(item, null));
                            }
                        }
                        i++;
                    }
                }
            }
        }

        public List<T> FillModelFromTable<T>(DataTable table)
        {
            return this.FillModelFromTable<T>(table, false);
        }

        public List<T> FillModelFromProperties<T>(DataTable table)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    try
                    {
                        property.SetValue(model, table.GetValue(property.Name, i), null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Erro ao setar campo {property.Name}: {ex.Message}");
                    }
                }
                modelList.Add(model);
            }

            return modelList;
        }

        public List<T> FillModelFromTableAccordingToValue<T>(DataTable table, bool showProgressBar, string columnName, object okValue, bool showColumnError = true)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            ProgressBar pgb = null;
            if (showProgressBar)
            {
                pgb = SBOApp.Application.StatusBar.CreateProgressBar("Carregando dados da tela", table.Rows.Count, false);
            }
            ModelControllerAttribute modelController;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (showProgressBar)
                {
                    pgb.Value++;
                }

                if (table.GetValue(columnName, i).ToString() != okValue.ToString())
                {
                    continue;
                }

                model = Activator.CreateInstance<T>();
                // Seta os valores no model
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        modelController = attribute as ModelControllerAttribute;
                        if (modelController != null)
                        {
                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                modelController.UIFieldName = modelController.Description;
                            }

                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                break;
                            }
                            else
                            {
                                try
                                {
                                    property.SetValue(model, table.GetValue(modelController.UIFieldName, i), null);
                                }
                                catch
                                {
                                    try
                                    {
                                        if (property.PropertyType == typeof(string))
                                        {
                                            property.SetValue(model, table.GetValue(modelController.UIFieldName, i).ToString(), null);
                                        }
                                        else if (property.PropertyType == typeof(int))
                                        {
                                            property.SetValue(model, Convert.ToInt32(table.GetValue(modelController.UIFieldName, i).ToString()), null);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (showColumnError)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                modelList.Add(model);
            }
            if (pgb != null)
            {
                pgb.Stop();
                Marshal.ReleaseComObject(pgb);
                pgb = null;
            }

            return modelList;
        }

        public List<T> FillModelFromTable<T>(DataTable table, bool showProgressBar)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            ProgressBar pgb = null;
            if (showProgressBar)
            {
                pgb = SBOApp.Application.StatusBar.CreateProgressBar("Carregando dados da tela", table.Rows.Count, false);
            }
            ModelControllerAttribute modelController;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (showProgressBar)
                {
                    pgb.Value++;
                }
                model = Activator.CreateInstance<T>();
                // Seta os valores no model
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        modelController = attribute as ModelControllerAttribute;
                        if (modelController != null)
                        {
                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                modelController.UIFieldName = modelController.Description;
                            }

                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                break;
                            }
                            else
                            {
                                property.SetValue(model, table.Columns.Item(modelController.UIFieldName).Cells.Item(i).Value, null);
                            }
                        }
                    }
                }
                modelList.Add(model);
            }
            if (pgb != null)
            {
                pgb.Stop();
                Marshal.ReleaseComObject(pgb);
                pgb = null;
            }

            return modelList;
        }

        public List<T> FillModelFromTable<T>(Grid grid, bool showProgressBar, SelectedRows selectedRows)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            DataTable table = grid.DataTable;

            ProgressBar pgb = null;
            if (showProgressBar)
            {
                pgb = SBOApp.Application.StatusBar.CreateProgressBar("Carregando dados da tela", table.Rows.Count, false);
            }
            ModelControllerAttribute modelController;
            foreach (int index in selectedRows)
            {
                if (showProgressBar)
                {
                    pgb.Value++;
                }
                model = Activator.CreateInstance<T>();
                // Seta os valores no model
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        modelController = attribute as ModelControllerAttribute;
                        if (modelController != null)
                        {
                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                modelController.UIFieldName = modelController.Description;
                            }

                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                break;
                            }
                            else
                            {
                                property.SetValue(model, table.GetValue(modelController.UIFieldName, grid.GetDataTableRowIndex(index)), null);
                            }
                        }
                    }
                }
                modelList.Add(model);
            }
            if (pgb != null)
            {
                pgb.Stop();
                Marshal.ReleaseComObject(pgb);
                pgb = null;
            }

            return modelList;
        }

        public List<T> FillModelFromGrid<T>(Grid grid)
        {
            DataTable table = grid.DataTable;
            return this.FillModelFromTable<T>(table);
        }

        /// <summary>
        /// Preenche lista de acordo com valor em determinada coluna
        /// </summary>
        /// <param name="columnName">Nome da coluna que irá retornar na lista</param>
        /// <param name="clmToCheck">Nome da coluna para verificar o valor</param>
        /// <param name="valueToCheck">Valor a ser verificado</param>
        /// <param name="table">Tabela</param>
        /// <returns></returns>
        public List<string> FillListAccordingToValue(string columnName, string clmToCheck, string valueToCheck, DataTable table)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.GetValue(clmToCheck, i).ToString() == valueToCheck)
                {
                    list.Add(table.GetValue(columnName, i).ToString());
                }
            }

            return list;
        }

        public List<T> FillModelFromGrid<T>(Grid grid, List<int> rows)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;
            DataTable table = grid.DataTable;
            ModelControllerAttribute modelController;
            for (int i = 0; i < rows.Count; i++)
            {
                model = Activator.CreateInstance<T>();
                // Seta os valores no model
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        modelController = attribute as ModelControllerAttribute;
                        if (modelController != null)
                        {
                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                modelController.UIFieldName = modelController.Description;
                            }

                            if (String.IsNullOrEmpty(modelController.UIFieldName))
                            {
                                break;
                            }
                            else
                            {
                                property.SetValue(model, table.GetValue(modelController.UIFieldName, i), null);
                            }
                        }
                    }
                }
                modelList.Add(model);
            }

            return modelList;
        }
    }
}
