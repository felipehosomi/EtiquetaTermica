using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Controllers;

namespace CVA.AddOn.Common.Models
{
    public class DefaultMasterDataModel
    {
        [ModelController()]
        public string Code { get; set; }

        [ModelController()]
        public string Name { get; set; }
    }
}
