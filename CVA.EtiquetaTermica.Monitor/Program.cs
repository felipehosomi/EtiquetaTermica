using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CVA.EtiquetaTermica.Monitor
{
    class Program
    {

        static Mutex mutex = new Mutex(true, "{a9227ba4-077f-4d65-b58a-8ec38c2d35f1}");
        [STAThread]
        static void Main(string[] args)
        {

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                var monitor = new Monitor();
                mutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine("Já existe uma instância do monitor em execução.");
                Console.ReadKey();
            }
        }
    }
}
