using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace CVA.EtiquetaTermica.Monitor2
{
    class Program
    {
        private static readonly string CaminhoOrigem = ConfigurationManager.AppSettings["CaminhoOrigem"];
        private static readonly string CaminhoDestino = ConfigurationManager.AppSettings["CaminhoDestino"];
        private static readonly int DelayDeEnvio = Convert.ToInt32(ConfigurationManager.AppSettings["DelayDeEnvio"]);
        private static readonly int DelayDeLeitura = Convert.ToInt32(ConfigurationManager.AppSettings["DelayDeLeitura"]);
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly Mutex mutex = new Mutex(true, $"CVA.EtiquetaTermica.Monitor2_{CaminhoOrigem.Replace('\\', '_')}");
        private const int NumberOfDeleteRetries = 3;
        private const int DelayOnDeleteRetry = 1000;

        [STAThread]
        static void Main(string[] args)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Start();
                mutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine($"Já existe uma instância do serviço em execução para o diretório \"{CaminhoOrigem}\"");
                Thread.Sleep(8000);
            }
        }

        private static void Start()
        {
            Directory.CreateDirectory(CaminhoOrigem);
            Console.Title = $"Monitor de impressão de etiquetas CVA - \"{CaminhoOrigem}\"";
            Logger.Info($"Monitorando diretório: \"{CaminhoOrigem}\"");
            Logger.Info($"Impressora de destino: \"{CaminhoDestino}\"");
            Logger.Info($"Delay de envio entre arquivos (ms): {DelayDeEnvio}");
            Logger.Info($"Delay entre verificações de novos arquivos no diretório de etiquetas (ms): {DelayDeLeitura}");
            bool logWaitingFiles = true;

            while (true)
            {
                var files = Directory.GetFiles(CaminhoOrigem).OrderBy(x => x);

                print:
                foreach (string file in files)
                {
                    Logger.Info($"Arquivo \"{file}\" captado");
                    CopyFileToPrinter(file);
                    Thread.Sleep(DelayDeEnvio);
                    logWaitingFiles = true;
                }

                // Verifica novamente se ainda existem arquivos pendentes de impressão
                if (files.Count() > 0)
                {
                    files = Directory.GetFiles(CaminhoOrigem).OrderBy(x => x);
                    goto print;
                }

                if (logWaitingFiles)
                {
                    Logger.Info("Aguardando arquivos...");
                    logWaitingFiles = false;
                }

                Thread.Sleep(DelayDeLeitura);
            }
        }

        private static void CopyFileToPrinter(string file)
        {
            try
            {
                File.Copy(file, Path.Combine(CaminhoDestino, Path.GetFileName(file)), true);
                Logger.Info($"Arquivo \"{file}\" enviado");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Arquivo \"{file}\" não enviado:");
            }
            finally
            {
                for (int i = 1; i <= NumberOfDeleteRetries; ++i)
                {
                    try
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(file);
                        Logger.Info($"Arquivo \"{file}\" apagado");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Arquivo \"{file}\" não apagado (tentativa {i}): {ex.Message}");
                        Thread.Sleep(DelayOnDeleteRetry);
                    }
                }
            }
        }
    }
}