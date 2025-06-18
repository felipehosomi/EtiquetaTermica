using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.EtiquetaTermica.Model
{
    public class TagModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class TagFullModel : TagModel
    {
        public string U_descricao { get; set; }
        public string U_script { get; set; }
        public string U_linguagem { get; set; }
        public string U_procedure { get; set; }
        public string U_local { get; set; }
        public string U_procpesq { get; set; }
        public string U_caminhoscript { get; set; }
    }

    public class TagResult
    {
        public Int32 Id { get; set; }
        public string Tag { get; set; }
    }

    public class TagParameterModel
    {
        public string ObjType { get; set; }
        public string Key { get; set; }
        public string cl1 { get; set; }
        public string cl2 { get; set; }
        public string cl3 { get; set; }
        public string cl4 { get; set; }
        public string cl5 { get; set; }
        public string cl6 { get; set; }
        public string cl7 { get; set; }
        public string cl8 { get; set; }
        public int linha { get; set; }
    }
}
