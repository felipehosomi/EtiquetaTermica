using CVA.AddOn.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.EtiquetaTermica.View
{
    public class fM1002 : BaseFormParent
    {
        public fM1002(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001002 form = new f2000001002();
                form.FormID = 1002;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }

    }
}
