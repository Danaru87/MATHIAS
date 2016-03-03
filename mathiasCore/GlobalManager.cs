using mathiasCore.DB;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathiasCore
{
    public static class GlobalManager
    {
        private static String DBPATH { get; set; }
        private static String DBFILE { get; set; }
        public static String SQLCHAIN { get; set; }

        public static void InitMathias()
        {
            Console.WriteLine("Verification des fichiers en cours...");

            DBPATH = String.Format("{0}\\database", Directory.GetCurrentDirectory());
            DBFILE = "mathias.sqlite";
            bool needInstall = false;
            if (!Directory.Exists(DBPATH))
            {
                needInstall = true;
                Directory.CreateDirectory(DBPATH);
                Console.WriteLine("Création du dossier " + DBPATH);
            }
            Console.WriteLine("Dossier de base de donnée vérifié");
            if (!File.Exists(String.Format(DBPATH + "\\{0}", DBFILE)))
            {
                needInstall = true;
                SQLiteConnection.CreateFile(String.Format(DBPATH + "\\{0}", DBFILE));
                Console.WriteLine("Création du fichier " + String.Format(DBPATH + "\\{0}", DBFILE));
            }
            Console.WriteLine("Fichier de base de donnée vérifié");
            SQLCHAIN = String.Format("Data Source = {0}; Version = 3;", String.Format(DBPATH + "\\{0}", DBFILE));
            Console.WriteLine("Chaine de connection crée: " + SQLCHAIN);
            System.Threading.Thread.Sleep(1000);
            DBManager.CreateDataBase();
        }
    }
}
