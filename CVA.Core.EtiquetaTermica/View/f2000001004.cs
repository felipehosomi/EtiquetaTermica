using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.EtiquetaTermica.DAO;
using CVA.Core.EtiquetaTermica.Model;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace CVA.Core.EtiquetaTermica.View
{
    /// <summary>
    /// Visualização de etiqueta
    /// </summary>
    class f2000001004 : BaseForm
    {
        public f2000001004()
        {
            FormCount++;
        }

        public f2000001004(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001004(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001004(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        public object Show(string imagePath)
        {
            Form form = (Form)this.Show();

            PictureBox pbx = (PictureBox)Form.Items.Item("pb_Etiq").Specific;
            pbx.Picture = imagePath;

            return form;
        }
    }
}
