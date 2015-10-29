using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace MATHIAS_beta
{
    public partial class Form1 : Form
    {
        private KinectSensor kinect = null;

        private KinectAudioStream convertStream;

        private SpeechRecognitionEngine speechEngine;
        private RecognizerInfo ri;

        private SpeechSynthesizer synt = new SpeechSynthesizer();
        

        public Form1()
        {
            InitializeComponent();
            synt.SetOutputToDefaultAudioDevice();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Detection de la kinect en cours";
            kinect = KinectSensor.GetDefault();
            
            ri = TryGetKinectRecognizer();

            ListenKinect();
            
                
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;
            {
                label2.Text = e.Result.Text;
                switch (e.Result.Text)
                {
                    case "Bonjour":
                    case "Salut":
                        synt.Speak("Bonjour");
                        break;
                    case "quitter":
                        synt.Speak("A bientôt");
                            Application.Exit();
                        
                        break;
                }
            }
            label3.Text = "Etat de la kinect :" + kinect.IsAvailable.ToString();
            ListenKinect();
        }


        private void ListenKinect()
        {
            if (kinect != null)
            {
                label3.Text = "Etat de la kinect :" + kinect.IsAvailable.ToString();
                if (kinect.IsOpen == false)
                {
                    kinect.Open();
                }

                // grab the audio stream
                IReadOnlyList<AudioBeam> audioBeamList = kinect.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // create the convert stream
                convertStream = new KinectAudioStream(audioStream);

                

                if (ri != null)
                {
                    speechEngine = new SpeechRecognitionEngine(ri.Id);

                    label1.Text = "Dernier mot reconnu : ";


                    Choices word = new Choices();
                    word.Add(new string[] { "oui", "non", "oui non", "adieu", "Salut", "chalut", "téléphone", "choux", "poux", "quitter" });

                    GrammarBuilder gb = new GrammarBuilder();
                    gb.Append(word);
                    gb.Culture = CultureInfo.GetCultureInfo("fr-FR");
                    Grammar gr = new Grammar(gb);

                    speechEngine.LoadGrammar(gr);

                    speechEngine.SpeechRecognized += SpeechRecognized;
                    speechEngine.SpeechRecognitionRejected += SpeechRejected;

                    convertStream.SpeechActive = true;

                    speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                    speechEngine.RecognizeAsync(RecognizeMode.Single);
                    label3.Text = "Dictionnaire chargé !";
                    
                    

                }
                else
                {
                    MessageBox.Show("No Engine");
                }
            }
            else
            {
                MessageBox.Show("Disconnected");
            }
        }
        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
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

        /// <summary>
        /// Handler for rejected speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            ListenKinect();
            label3.Text = "Etat de la kinect :" + kinect.IsAvailable.ToString();
        }

    }
}
