using Microsoft.Phone.Controls;
using System;
using System.Windows;
using Windows.Phone.Speech.Recognition;

namespace Voice
{
    public partial class SpeechToText : PhoneApplicationPage
    {
        public SpeechToText()
        {
            InitializeComponent();
        }

        private async void SpeakNote_Click(object sender, RoutedEventArgs e)
        {
            // Spracherkennung initialisieren
            var sr = new SpeechRecognizerUI();
            sr.Settings.ListenText = "Notiz erfassen";
            sr.Settings.ExampleText = "Geburtstagsgeschenk kaufen";
            sr.Settings.ReadoutEnabled = true;
            sr.Settings.ShowConfirmation = false;

            // Spracherkennungsergebnis abfragen
            var result = await sr.RecognizeWithUIAsync();
            if (result.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                // erfolgreich - erkannten Text und Genaugigkeit ausgeben
                string spokenText = result.RecognitionResult.Text;
                string confidence = result.RecognitionResult.TextConfidence.ToString();

                SpokenText.Text = spokenText;
                Status.Text = confidence;
            }
            else
            {
                // nicht erfolgreich - Status ausgeben
                Status.Text = result.ResultStatus.ToString();
            }
        }

        private async void AskQuestion_Click(object sender, RoutedEventArgs e)
        {
            // Spracherkennung initialisieren
            var sr = new SpeechRecognizerUI();
            sr.Settings.ListenText = "Welcher Tag ist heute?";
            sr.Settings.ReadoutEnabled = true;
            sr.Settings.ShowConfirmation = false;

            // Erkennbare Wörter reduzieren auf Werktage
            var weekdays = new[] { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag" };
            sr.Recognizer.Grammars.AddGrammarFromList("Weekdays", weekdays);

            // Spracherkennungsergebnis abfragen
            var result = await sr.RecognizeWithUIAsync();
            if (result.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                // erfolgreich - erkannten Text und Genaugigkeit ausgeben
                string spokenText = result.RecognitionResult.Text;
                string confidence = result.RecognitionResult.TextConfidence.ToString();

                SpokenText.Text = spokenText;
                Status.Text = confidence;
            }
            else
            {
                // nicht erfolgreich - Status ausgeben
                Status.Text = result.ResultStatus.ToString();
            }
        }
    }
}