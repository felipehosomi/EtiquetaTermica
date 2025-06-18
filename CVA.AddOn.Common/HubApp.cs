using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Enums;

namespace CVA.AddOn.Common
{
    public class CVAApp
    {
        public static string ServerName { get; set; }

        public static string DatabaseName { get; set; }

        public static string DBUserName { get; set; }

        public static string DBPassword { get; set; }

        public static AppTypeEnum AppType { get; set; }

        public static DatabaseTypeEnum DatabaseType { get; set; }

        public static void FillConnectionParameters()
        {

        }
    }
}
