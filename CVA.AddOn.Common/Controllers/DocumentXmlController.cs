using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CVA.AddOn.Common.Controllers
{
    public class DocumentXmlController
    {
        public static string GetXmlField(BusinessObjectInfo businessObjectInfo, string field = "DocEntry")
        {

            XmlDocument oXmlDoc = new XmlDocument();
            oXmlDoc.LoadXml(businessObjectInfo.ObjectKey);

            XmlNodeList oXmlNodeList = oXmlDoc.GetElementsByTagName(field);
            string ValorChave = oXmlNodeList[0].InnerText;

            return ValorChave;
        }

    }
}
