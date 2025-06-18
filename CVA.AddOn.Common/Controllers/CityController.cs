using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Models;

namespace CVA.AddOn.Common.Controllers
{
    public class CityController : BaseController
    {
        public CityController()
            : base("OCNT")
        { }

        public List<CityModel> GetCitiesList(string country, string uf)
        {
            string where = "Country = '{0}' AND State = '{1}'";
            where = String.Format(where, country, uf);

            List<CityModel> list = this.RetrieveModelList<CityModel>(where);
            return list;
        }
    }
}
