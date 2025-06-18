using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Controllers;

namespace CVA.AddOn.Common.Models
{
    public class BusinessPlaceModel
    {
        [ModelController]
        public int BPlId { get; set; }
        
        [ModelController]
        public string BPlName { get; set; }

        [ModelController(ColumnName = "TaxIdNum")]
        public string Cnpj { get; set; }

        [ModelController(ColumnName = "AddtnlId")]
        public string InscMunicipal { get; set; }
    }
}
