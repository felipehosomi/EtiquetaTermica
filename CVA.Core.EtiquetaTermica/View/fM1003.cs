using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.EtiquetaTermica.View
{
    class fM1003 : BaseFormParent
    {
        public fM1003(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001003 form = new f2000001003();
                form.FormID = 1003;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
