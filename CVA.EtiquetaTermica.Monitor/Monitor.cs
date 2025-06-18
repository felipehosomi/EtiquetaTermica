using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CVA.EtiquetaTermica.Monitor
{
    public class Monitor
    {

        private static string origem = CVA.EtiquetaTermica.Monitor.Properties.Settings.Default.origem;
        private static string destino = CVA.EtiquetaTermica.Monitor.Properties.Settings.Default.destino;
        private bool ativo = false;
        private List<string> toDelete = new List<string>();

        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        public Monitor()
        {
            Log("* Monitor iniciado com sucesso.");
            Log("* CVA.EtiquetaTermica.Monitor START");
            Log("----------------------------");
            Log("* Origem: " + origem);
            Log("* Destino: " + destino);
            Log("----------------------------");

            if (!System.IO.Directory.Exists(origem))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(origem);
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    Console.ReadKey();
                    return;
                }
            }


            Thread t = new Thread(GetFiles);
            t.Start();

            Log("* Thread princial iniciada.");

            System.Timers.Timer timer = new System.Timers.Timer(5000);
            timer.Elapsed += PrintFiles;
            timer.Start();

            Log("* Monitor iniciado com sucesso.");

        }

        void PrintFiles(object sender, ElapsedEventArgs e)
        {
            //Thread.Sleep(10000);
            var lista = toDelete.ToList();

            Console.Write("x");

            foreach (var item in lista)
            {

                try
                {
                    // Thread.Sleep(1000);
                    System.IO.File.Delete(item);
                    toDelete.Remove(item);
                    Log("DEL " + item);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private void GetFiles()
        {
            do
            {
                if (!ativo)
                {

                    Console.Write(".");

                    try
                    {
                        Verifica();
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message);
                    }
                }

                Thread.Sleep(1000);

            } while (1 == 1);

        }

        private void Verifica()
        {

            var files = from c in System.IO.Directory.GetFiles(origem)
                        where !toDelete.Contains(c)
                        select c;


            foreach (var item in files)
            {
                Log("SEND " + item + " to " + destino);
                try
                {

                    System.IO.File.Copy(item, destino);

                    toDelete.Add(item);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }



    }
}
