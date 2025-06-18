using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Enums;

namespace CVA.AddOn.Common.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelBaseControllerAttribute : Attribute
    {
        public EnumModelTableType EnumModelTableType { get; set; }
    }
}
