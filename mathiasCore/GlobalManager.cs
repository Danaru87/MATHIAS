using mathiasCore.DB;
using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mathiasCore
{
    public static class GlobalManager
    {
        #region DATABASE PROP
        private static String DBPATH { get; set; }
        private static String DBFILE { get; set; }
        public static String SQLCHAIN { get; set; }
        #endregion

        #region KINECT CORE PROP
        public static Grammar GRAMMAR { get; set; }
        public static RecognizerInfo RI { get; set; }
        #endregion

        #region MATHIAS GLOBAL CONFIG
        public static Dictionary<String, String> CONFIGURATION { get; set; }
        #endregion

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

            // Construction du RI
            RI = TryGetKinectRecognizer();

            // Chargement du contexte par defaut
            GetGrammar();

        }

        public static void GetGrammar(string ContextName = null)
        {
            var gb = new GrammarBuilder { Culture = RI.Culture };
            Choices commands;
            commands = DBContext.GetGrammar();
            gb.Append(commands);
            GRAMMAR = new Grammar(gb);
        }

        /// <summary>
        /// Get Recognizer from Kinect & System
        /// </summary>
        /// <returns>RecognizerInfo</returns>
        private static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "fr-FR".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }
            return null;
        }
    }
}
