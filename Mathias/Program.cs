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
using mathiasModels.Xtend;

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
        static string LastAction = "";

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

            List<InstalledVoice> voices = speaker.GetInstalledVoices().ToList();
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
                speechEngine.LoadGrammar((Grammar)GlobalManager.CONTEXT.GRAMMAR);
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

            while(GlobalManager.RUNNING)
            {
            }

            if (!GlobalManager.RUNNING)
            {
                speaker.Speak("Au revoir");
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
            

            const double ConfidenceThreshold = 0.70;

            if (e.Result.Confidence >= ConfidenceThreshold && !LastAction.Equals(e.Result.Semantics.Value.ToString()))
            {
                if (GlobalManager.STANDBY == false)
                {
                    Console.WriteLine("Phrase reconnue: " + e.Result.Text);
                    PlugResponse response = GlobalManager.FireAction(e.Result.Semantics.Value.ToString(), e.Result.Text);
                    if (!e.Result.Semantics.Value.ToString().Equals("EXIT"))
                    {
                        speaker.Speak(response.Response);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                else
                {
                    
                    switch(e.Result.Semantics.Value.ToString())
                    {
                        case "ON":
                            speaker.Speak("Réveil en cours");
                            speaker.Speak("Je suis prêt à vous obéir");
                            GlobalManager.STANDBY = false;
                            break;
                    }

                }
                //System.Threading.Thread.Sleep(1000);
                
            }
            else if (LastAction.Equals(e.Result.Semantics.Value.ToString()))
            {
                speaker.Speak("Je viens de te répondre...");
            }
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
