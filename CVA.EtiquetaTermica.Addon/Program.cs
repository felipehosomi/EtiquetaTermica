using System;
using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using System.Windows.Forms;
using CVA.Core.EtiquetaTermica.BLL;

namespace CVA.EtiquetaTermica.Addon
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Exit();
                return;
            }

            var sboApp = new SBOApp(args[0], Application.StartupPath + "\\CVA.Core.EtiquetaTermica.dll");
            sboApp.InitializeApplication();

            EventFilterController.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();            

            MenuController.LoadFromXML(Application.StartupPath + "\\Menu.xml");

            ListenerController oListener = new ListenerController();
            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            System.Windows.Forms.Application.Run();
        }
    }
}
