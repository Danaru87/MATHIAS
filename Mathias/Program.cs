using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using mathiasCore;
using System.Runtime.InteropServices;
using mathiasCore.DB;
using AE.Net.Mail;

namespace Mathias
{
    class Program
    {
        static KinectSensor kinectSensor;
        static KinectAudioStream convertStream;
        static SpeechRecognitionEngine speechEngine;
        static SpeechSynthesizer speaker;
        static bool active;
        static String DefaultContext = "General";

        public static string DBPATH { get; private set; }
        public static object DBFILE { get; private set; }
        public static bool RUNNING { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenu dans Mathias");
            RUNNING = true;
            active = true;
            GlobalManager.InitMathias();

            Console.WriteLine("Initialisation de la Kinect");
            speaker = new SpeechSynthesizer();

            List<InstalledVoice> voices = speaker.GetInstalledVoices().ToList(); ;
            Console.WriteLine(speaker.Voice.Name);

            speaker.Speak("Démarrage en cours");
            kinectSensor = KinectSensor.GetDefault();
            if(kinectSensor != null)
            {
                Console.WriteLine("La kinect est récupérée");
                kinectSensor.Open();
                Console.WriteLine("La kinect est prête à recevoir les informations");

                Console.WriteLine("Récupération de l'audio beam");
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();
                Console.WriteLine("Stream et audio beam OK");

                Console.WriteLine("Conversion de l'audioStream");
                convertStream = new KinectAudioStream(audioStream);
                Console.WriteLine("Conversion OK");
            }
            else { Console.WriteLine("Impossible de récupérer la kinect"); }

            
            Console.WriteLine(GlobalManager.RI.Name + "Récupéré");

            if (GlobalManager.RI != null)
            {
                Console.WriteLine("Construction du grammar sample");
                speechEngine = new SpeechRecognitionEngine(GlobalManager.RI.Id);
                Console.WriteLine("Construction du grammar terminée");
                speechEngine.LoadGrammar(GlobalManager.GRAMMAR);
                speechEngine.SpeechRecognized += SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += SpeechRejected;

                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                Console.WriteLine("Il ne reste plus qu'a parler");
            }
            else
            {
                Console.WriteLine("Could not find speech recognizer");
            }
            while(RUNNING)
            {
            }
        }

        private static void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (active)
            {
                Console.WriteLine("Aucune phrase reconnue");
                            speaker.Speak("Je n'ai pas reconnu votre phrase");
                            System.Threading.Thread.Sleep(1000);
            }
            
        }

        private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            const double ConfidenceThreshold = 0.75;

            if(e.Result.Confidence >= ConfidenceThreshold)
            {
                if (active)
                {
                    //Fire(e.Result.Semantics.Value.ToString());
                    switch (e.Result.Semantics.Value.ToString())
                    {
                        case "HELLO":
                            speaker.Speak("Bonjour copain !");
                            Console.WriteLine("Bonjour à vous !");
                            break;
                        case "HUMEUR":
                            speaker.Speak("Oui, et toi ?");
                            Console.WriteLine("Oui et toi ?");
                            break;
                        case "roux":
                            speaker.Speak("Roux, Juif, et pédophile...");
                            Console.WriteLine("Roux...");
                            break;
                        case "READ EMAIL":
                            speaker.Speak("Chargement du message");
                            string email = GetEmail("arnaud.dasilva@openmailbox.org","wakete86");
                            speaker.Speak(email);//TODO: Appeler méthode de lecture
                            break;
                        case "EXIT":
                            speaker.Speak("A bientôt !");
                            Console.WriteLine("ADIOS!");
                            RUNNING = false;
                            break;
                        case "OFF":
                            speaker.Speak("Mis en veille activée");
                            Console.WriteLine("Mise en veille");
                            active = false;
                            break;
                    }
                }
                else
                {
                    switch(e.Result.Semantics.Value.ToString())
                    {
                        case "ON":
                            speaker.Speak("Réveil en cours");
                            speaker.Speak("Je suis prêt à vous obéir");
                            active = true;
                            break;
                    }
                }
                System.Threading.Thread.Sleep(1000);
                
            }
        }

        private static void Fire(string v)
        {
            // Todo: recherche une correspondance dans la BDD
            // Si correspondance trouvée:*
                // Chargement de la DLL concernée
                // Création de l'objet PlugCall
                // Lancement de la méthode concernée
            // Si non
                // Indiquer aucune commande correspondante.
            throw new NotImplementedException();
        }

        private static string GetEmail(string v1, string v2)
        {
            ImapClient client = new ImapClient("imap.openmailbox.org", v1, v2, AuthMethods.Login, 993, true);

            client.SelectMailbox("INBOX");

            MailMessage[] listemail = client.GetMessages(0,20);
            MailMessage email = listemail[12];
            if(String.IsNullOrEmpty(email.Body))
            {
                string body = client.GetMessage(email.Uid).Subject;
                client.Dispose();
                return body;
            }
            client.Dispose();
            return email.Subject;
            
        }
    }
}
