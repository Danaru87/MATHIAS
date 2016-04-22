using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathiasCore.DB
{
    public static class DBManager
    {
        public static String SQLCHAIN { get; private set; }
        public static SQLiteConnection SQLClient { get; private set; }
         

        public static bool CreateDataBase()
        {
            bool success = true;
            String iniPath = Path.Combine(Directory.GetCurrentDirectory().ToString(), "Scripts\\DbInstall.ini");
            StreamReader file = new StreamReader(iniPath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string Queries = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), String.Format("Scripts\\{0}", line)));
                    Queries = Queries.Replace("\n", "");
                    Queries = Queries.Replace("\r", "");
                    Queries = Queries.Replace("\t", " ");
                    string[] queriesList = Queries.Split(';');
                    foreach (String oneQ in queriesList)
                    {
                        using (SQLiteConnection sqlite = new SQLiteConnection(GlobalManager.SQLCHAIN))
                        {
                            sqlite.Open();
                            sqlite.Execute(oneQ);
                            sqlite.Close();
                        }
                        Console.WriteLine(Queries);
                    }
                    
                }
                catch (Exception e)
                {
                    success = false;
                    Console.WriteLine("Exception: " + e.Message);
                }

            }
            return success;
        }
    }
}
