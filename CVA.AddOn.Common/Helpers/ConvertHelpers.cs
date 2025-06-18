using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CVA
{
    public static class ConvertHelper
    {
        public static int? ToIntNull(this object vObj)
        {
            try
            {
                int? vRet = null;
                int vRetNotNull = 0;

                if (vObj != null && int.TryParse(vObj.ToString(), out vRetNotNull))
                    vRet = vRetNotNull;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ToInt(this object vObj)
        {
            try
            {
                int vRet = vObj.ToIntNull().Value;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double? ToDoubleNull(this object vObj)
        {
            try
            {
                double? vRet = null;
                double vRetNotNull = 0;

                if (vObj != null && double.TryParse(vObj.ToString(), out vRetNotNull))
                    vRet = vRetNotNull;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double ToDouble(this object vObj)
        {
            try
            {
                double vRet = vObj.ToDoubleNull().Value;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? ToDecimalNull(this object vObj)
        {
            try
            {
                decimal? vRet = null;
                decimal vRetNotNull = 0;

                if (vObj != null && decimal.TryParse(vObj.ToString(), out vRetNotNull))
                    vRet = vRetNotNull;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal ToDecimal(this object vObj)
        {
            try
            {
                decimal vRet = vObj.ToDecimalNull().Value;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime? ToDateTimeNull(this object vObj)
        {
            try
            {
                DateTime? vRet = null;
                DateTime vRetNotNull = new DateTime();

                if (vObj != null && DateTime.TryParse(vObj.ToString(), out vRetNotNull))
                    vRet = vRetNotNull;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime ToDateTime(this object vObj)
        {
            try
            {
                DateTime vRet = vObj.ToDateTimeNull().Value;

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ToAlphaNumericOnly(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(input, "");
        }

        public static string ToAlphaOnly(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z]");
            return rgx.Replace(input, "");
        }

        public static string ToNumericOnly(this string input)
        {
            Regex rgx = new Regex("[^0-9]");
            return rgx.Replace(input, "");
        }
    }
}