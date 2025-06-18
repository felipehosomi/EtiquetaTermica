using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace CVA.AddOn.Common.Controllers
{
    public class MatrixController
    {
        #region AddMatrixRow
        public void AddMatrixRow(IMatrix matrix, IDBDataSource dBDataSource, bool focusCell)
        {
            // Insere um novo registro vazio dentro do data source
            dBDataSource.InsertRecord(dBDataSource.Size);

            if (dBDataSource.Size == 1)
            {
                dBDataSource.InsertRecord(dBDataSource.Size);
            }

            if (matrix.VisualRowCount.Equals(0))
            {
                dBDataSource.RemoveRecord(0);
            }

            // Loads the user interface with current data from the matrix objects data source.
            matrix.LoadFromDataSourceEx(false);

            if (focusCell)
                matrix.SetCellFocus(matrix.VisualRowCount, 1);
        }
        #endregion

        #region RemoveMatrixRow
        public void RemoveMatrixRow(IMatrix matrix, IDBDataSource dBDataSource, int row)
        {
            dBDataSource.RemoveRecord(row - 1);

            // Loads the user interface with current data from the matrix objects data source.
            matrix.LoadFromDataSource();
        }
        #endregion

        public T FillModelFromMatrix<T>(Matrix mtx)
        {
            List<T> modelList = this.FillModelListFromMatrix<T>(mtx);
            if (modelList.Count == 0)
            {
                return Activator.CreateInstance<T>();
            }
            else
            {
                return modelList[0];
            }
        }

        public List<T> FillModelListFromMatrix<T>(Matrix mtx)
        {
            // Cria nova instância do model
            List<T> modelList = new List<T>();
            T model;

            ModelControllerAttribute modelController;

            for (int i = 1; i < mtx.RowCount + 1; i++)
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
                            if (!modelController.AutoFill)
                            {
                                continue;
                            }

                            if (String.IsNullOrEmpty(modelController.DataSourceFieldName))
                            {
                                modelController.DataSourceFieldName = property.Name;
                            }

                            string value = String.Empty;

                            switch (modelController.UIFieldType)
                            {
                                case CVA.AddOn.Common.Enums.UIFieldTypeEnum.EditText:
                                    EditText etx = (EditText)mtx.Columns.Item(modelController.UIFieldName).Cells.Item(i).Specific;
                                    value = etx.Value;
                                    break;
                                case CVA.AddOn.Common.Enums.UIFieldTypeEnum.PriceEditText:
                                    EditText etxPrice = (EditText)mtx.Columns.Item(modelController.UIFieldName).Cells.Item(i).Specific;
                                    value = etxPrice.Value.Split(' ')[0];
                                    break;
                                case CVA.AddOn.Common.Enums.UIFieldTypeEnum.ComboBox:
                                    ComboBox comboBox = (ComboBox)mtx.Columns.Item(modelController.UIFieldName).Cells.Item(i).Specific;
                                    value = comboBox.Value;
                                    break;
                                case CVA.AddOn.Common.Enums.UIFieldTypeEnum.CheckBox:
                                    CheckBox checkBox = (CheckBox)mtx.Columns.Item(modelController.UIFieldName).Cells.Item(i).Specific;
                                    if (checkBox.Checked)
                                    {
                                        value = checkBox.ValOn;
                                    }
                                    else
                                    {
                                        value = checkBox.ValOff;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            if (String.IsNullOrEmpty(value))
                            {
                                continue;
                            }

                            if (property.PropertyType == typeof(string))
                            {
                                property.SetValue(model, value, null);
                            }
                            else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(Nullable<double>))
                            {
                                property.SetValue(model, Convert.ToDouble(value.Replace(".", ",")), null);
                            }
                            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(Nullable<decimal>))
                            {
                                property.SetValue(model, Convert.ToDecimal(value.Replace(".", ",")), null);
                            }
                            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(Nullable<int>))
                            {
                                property.SetValue(model, Convert.ToInt32(value.Replace(".", ",")), null);
                            }
                            else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(Nullable<short>))
                            {
                                property.SetValue(model, Convert.ToInt16(value.Replace(".", ",")), null);
                            }
                            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(Nullable<bool>))
                            {
                                property.SetValue(model, Convert.ToBoolean(value), null);
                            }
                            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Nullable<DateTime>))
                            {
                                property.SetValue(model, DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.CurrentCulture), null);
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
