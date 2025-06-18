using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CVA.AddOn.Common.Controllers
{
    public class DataSourceController
    {
        public T FillModelFromDBDataSource<T>(DBDataSource dataSource)
        {
            List<T> modelList = this.FillModelListFromDBDataSource<T>(dataSource);
            if (modelList.Count == 0)
            {
                return Activator.CreateInstance<T>();
            }
            else
            {
                return modelList[0];
            }
        }

        public List<T> FillModelListFromDBDataSource<T>(DBDataSource dataSource)
        {
            // Cria nova instância do model
            List<T> modelList = new List<T>();
            T model;

            ModelControllerAttribute modelController;

            for (int i = 0; i < dataSource.Size; i++)
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

                            string value = dataSource.GetValue(modelController.DataSourceFieldName, 0);
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
