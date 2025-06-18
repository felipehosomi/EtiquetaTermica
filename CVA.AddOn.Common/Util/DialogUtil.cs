using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CVA.AddOn.Common.Util
{
    public class DialogUtil
    {
        private string resultString;

        /// <summary>
        /// Dialog para selecionar pasta
        /// </summary>
        /// <returns>Pasta Selecionada</returns>
        public string FolderBrowserDialog(string selectedPath = null)
        {
            Thread thread = new Thread(() => ShowFolderBrowserDialog(selectedPath));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        /// <summary>
        /// Dialog para selecionar arquivo
        /// </summary>
        /// <returns>Arquivo Selecionado</returns>
        public string OpenFileDialog(string filter = "")
        {
            Thread thread = new Thread(() => ShowOpenFileDialog(filter));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        private void ShowFolderBrowserDialog(string selectedPath = null)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(selectedPath))
            {
                fbd.SelectedPath = selectedPath;
            }

            if (fbd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = fbd.SelectedPath;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }

      
        private void ShowOpenFileDialog(string filter = "")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (!String.IsNullOrEmpty(filter))
            {
                ofd.Filter = filter;
            }
            if (ofd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = ofd.FileName;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}
