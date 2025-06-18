using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Controllers;

namespace CVA.AddOn.Common.Models
{
    public class ItemModel
    {
        [ModelController(ColumnName = "ItemCode")]
        public string ItemCode { get; set; }

        [ModelController(ColumnName = "ItemName")]
        public string ItemName { get; set; }

        [ModelController(ColumnName = "U_linha")]
        public string LabelLineProd { get; set; }
    }
}
