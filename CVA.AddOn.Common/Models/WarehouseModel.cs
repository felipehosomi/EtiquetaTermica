using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Controllers;

namespace CVA.AddOn.Common.Models
{
    public class WarehouseModel
    {
        [ModelController]
        public string WhsCode { get; set; }

        [ModelController]
        public string WhsName { get; set; }
    }
}
