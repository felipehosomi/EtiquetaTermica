using CVA.AddOn.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Common.Util
{
    public class ObjectTypeUtil
    {
        public static string GetTable(ObjectTypeEnum objectTypeEnum)
        {
            switch (objectTypeEnum)
            {
                case ObjectTypeEnum.NotaFiscalSaida:
                    return "INV";
                case ObjectTypeEnum.DevNotaFiscalSaida:
                    return "RIN";
                case ObjectTypeEnum.Entrega:
                    return "DLN";
                case ObjectTypeEnum.Devolucao:
                    return "RDN";
                case ObjectTypeEnum.PedidoVenda:
                    return "RDR";
                case ObjectTypeEnum.NotaFiscalEntrada:
                    return "PCH";
                case ObjectTypeEnum.DevNotaFiscalEntrada:
                    return "RPC";
                case ObjectTypeEnum.RecebimentoMercadorias:
                    return "PDN";
                case ObjectTypeEnum.DevolucaoMercadorias:
                    return "RPD";
                case ObjectTypeEnum.PedidoCompra:
                    return "POR";
                case ObjectTypeEnum.Cotacao:
                    return "QUT";
                case ObjectTypeEnum.OfertaCompra:
                    return "PQT";
            }
            throw new Exception("Objeto não encontrado!");
        }

        public static string GetDocumentTable(ObjectTypeEnum objectTypeEnum)
        {
            return "O" + GetTable(objectTypeEnum);
        }
    }
}
