using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVA.AddOn.Common.Models
{
    public class DocumentItemModel
    {
        public string ItemCode { get; set; }
        public int LineNum { get; set; }
        public int VisOrder { get; set; }
        public int DocEntry { get; set; }
        public double Quantity { get; set; }
        public double OpenQty { get; set; }
        public double Price { get; set; }
        public string Warehouse { get; set; }
        public string UoM { get; set; }
        public string Account { get; set; }
    }
}
