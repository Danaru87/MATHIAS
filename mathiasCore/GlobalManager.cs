using mathiasCore.DB;
using mathiasModels;
using mathiasModels.Xtend;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static RecognizerInfo RI { get; set; }
        #endregion

        #region MATHIAS GLOBAL CONFIG
        public static Dictionary<String, String> CONFIGURATION { get; set; }
        public static Context CONTEXT { get; private set; }
        #endregion

        #region
        public static PlugResponse LastResponse { get; set; }
        #endregion

        /// <summary>
        /// Initialise base component for Mathias
        /// Check Databse / Install Database
        /// </summary>
        public static void InitMathias()
        {
            ///Vérification de la base de données
            CheckDatabase();

            // Construction du RI
            RI = TryGetKinectRecognizer();

            // Chargement du contexte par defaut
            LoadGrammar(null);

        }

        private static void CheckDatabase()
        {
            Console.WriteLine("Verification des fichiers en cours...");

            ///Construction du chemin de la base de données
            DBPATH = String.Format("{0}\\database", Directory.GetCurrentDirectory());
            DBFILE = "mathias.sqlite";

            /// Vérification de la nécéssité d'installation d'une nouvelle base
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

            ///Création de la chaine de connection vers la base de données
            SQLCHAIN = String.Format("Data Source = {0}; Version = 3;", String.Format(DBPATH + "\\{0}", DBFILE));
            Console.WriteLine("Chaine de connection crée: " + SQLCHAIN);
            System.Threading.Thread.Sleep(1000);

            ///Création de la base de données
            DBManager.CreateDataBase();

            CONTEXT = DBContext.GetContext();
        }


        /// <summary>
        /// Récupération du grammar / contexte
        /// </summary>
        /// <param name="ContextName"></param>
        public static void LoadGrammar(string ContextName = null)
        {
            // Chargement du GRAMMAR Système
            Choices Choices = new Choices();
            GrammarBuilder Builder = new GrammarBuilder { Culture = RI.Culture };
            foreach(SENTENCES action in DBContext.GetContext(ContextName).SENTENCESLIST)
            {
                Choices.Add(new SemanticResultValue(action.SENTENCE, action.CMD.CMD));
            }
            Builder.Append(Choices);
            CONTEXT.GRAMMAR = new Grammar(Builder);
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

        public static PlugResponse FireAction(string ActionName, string sentence)
        {
            PlugResponse response = new PlugResponse();
            if (LastResponse != null && LastResponse.WaitForChainedAction == true)
            {
                Console.WriteLine("Reponse à la question précèdente: " + LastResponse.ChainedQuestion);
            }
            SENTENCES tmp = CONTEXT.SENTENCESLIST.Where(t => t.CMD.CMD.Equals(ActionName)).Where(t=>t.SENTENCE.Equals(sentence)).Single();
            // Todo: recherche une correspondance dans la BDD
            if (tmp == null)
            {
                return new PlugResponse();
            }
            
            else
            {
                if (tmp.CMD.MODULE.NAME.Equals("SYSTEM"))
                {
                    PlugCall call = new PlugCall();
                    call.ACTION = ActionName;
                    response = SYSMODULE.DoAction(call);
                    LastResponse = response;
                    return response;
                }
                // Chargement de la DLL concernée
                var DLL = Assembly.LoadFile("");
                response = new PlugResponse();
                // LOAD DLL
            }
            
            // Création de l'objet PlugCall
            // Lancement de la méthode concernée
            // Si non
            // Indiquer aucune commande correspondante.

            return null;
        }
    }
}
