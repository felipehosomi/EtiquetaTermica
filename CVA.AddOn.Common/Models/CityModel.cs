using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Controllers;

namespace CVA.AddOn.Common.Models
{
    public class CityModel
    {
        [ModelController()]
        public int AbsId { get; set; }

        [ModelController()]
        public string Code { get; set; }

        [ModelController()]
        public string State { get; set; }

        [ModelController()]
        public string Name { get; set; }

        [ModelController()]
        public string TaxZone { get; set; }

        [ModelController()]
        public string IbgeCode { get; set; }

        [ModelController()]
        public string GiaCode { get; set; }
    }
}
