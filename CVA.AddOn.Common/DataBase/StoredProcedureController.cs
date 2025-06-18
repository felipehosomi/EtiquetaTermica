using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.DAO.Resources;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CVA.AddOn.Common.DataBase
{
    public class StoredProcedureController
    {
        private string Server;
        private string Database;
        private string User;
        private string Password;

        public StoredProcedureController()
        {

        }

        public StoredProcedureController(string server, string db, string user, string password)
        {
            this.Server = server;
            this.Database = db;
            this.User = user;
            this.Password = password;
        }

        public static void CreateSprocs(string path = "")
        {
            if (String.IsNullOrEmpty(path))
            {
                path = Path.Combine(Environment.CurrentDirectory, "DAO", "Sprocs");
            }
            foreach (string file in Directory.GetFiles(path, "*.sql"))
            {
                string sprocName = Path.GetFileNameWithoutExtension(file);
                if (CrudController.ExecuteScalar(String.Format(SQL.Sproc_Exists, sprocName)).ToString() == "0")
                {
                    try
                    {
                        StoredProcedureController.CreateProcedure(file);
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.SetStatusBarMessage($"Erro ao executar arquivo {file}: {ex.Message}");
                    }
                }
            }
        }

        public static void CreateProcedure(string file)
        {
            StreamReader reader;
            Regex r = new Regex(@"^(\s|\t)*go(\s\t)?.*", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            try
            {
                reader = new StreamReader(file);

                string sproc = reader.ReadToEnd();

                foreach (string s in r.Split(sproc))
                {
                    //Skip empty statements, in case of a GO and trailing blanks or something
                    string thisStatement = s.Trim();
                    if (String.IsNullOrEmpty(thisStatement))
                    {
                        continue;
                    }

                    CrudController.ExecuteNonQuery(s);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void CreateProcedureSQL(string file)
        {
            SqlController sqlController = new SqlController(this.Server, this.Database, this.User, this.Password);

            StreamReader reader;
            Regex r = new Regex(@"^(\s|\t)*go(\s\t)?.*", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            try
            {
                reader = new StreamReader(file);

                string sproc = reader.ReadToEnd();

                foreach (string s in r.Split(sproc))
                {
                    //Skip empty statements, in case of a GO and trailing blanks or something
                    string thisStatement = s.Trim();
                    if (String.IsNullOrEmpty(thisStatement))
                    {
                        continue;
                    }

                    sqlController.ExecuteNonQuery(s);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void CreateProcedureList(string path)
        {
            SqlController sqlController = new SqlController(this.Server, this.Database, this.User, this.Password);
            string[] filePaths = Directory.GetFiles(path);

            StreamReader reader;
            Regex r = new Regex(@"^(\s|\t)*go(\s\t)?.*", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string file in filePaths)
            {
                try
                {
                    reader = new StreamReader(file);

                    string sproc = reader.ReadToEnd();

                    foreach (string s in r.Split(sproc))
                    {
                        //Skip empty statements, in case of a GO and trailing blanks or something
                        string thisStatement = s.Trim();
                        if (String.IsNullOrEmpty(thisStatement))
                        {
                            continue;
                        }

                        sqlController.ExecuteNonQuery(s);
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
