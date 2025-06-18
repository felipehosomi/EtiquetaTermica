using CVA.AddOn.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.EtiquetaTermica.View
{
    public class fM1005 : BaseFormParent
    {
        public fM1005(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001001 form = new f2000001001();
                form.FormID = 1001;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }

    }
}
