using CVA.AddOn.Common.Controllers;


namespace CVA.Core.EtiquetaTermica.Model
{
    public class EtiquetaPNModel
    {
        [ModelController(UIFieldName = "Code")]
        public int Code { get; set; }
        [ModelController(UIFieldName = "Código")]
        public string CodigoEtiqueta { get; set; }
        [ModelController(UIFieldName = "Etiqueta Padrão")]

        public string EtiquetaPadrao { get; set; }
    }
}
