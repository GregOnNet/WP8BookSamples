using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows;
using Windows.Phone.Speech.Synthesis;

namespace Voice
{
    public partial class TextToSpeechPage : PhoneApplicationPage
    {
        public TextToSpeechPage()
        {
            InitializeComponent();
        }

        private void ReadAloud_Click(object sender, RoutedEventArgs e)
        {
            var sync = new SpeechSynthesizer();

            // auf Ende des Vorlesevorganges wird nicht gewartet
            sync.SpeakTextAsync(TextToRead.Text);
        }

        private async void ReadAloudWithOptions_Click(object sender, RoutedEventArgs e)
        {
            var sync = new SpeechSynthesizer();

            // als Sprache eine männliche, deutsche Stimme von den installierten Sprachen abrufen und setzen
            var voice = InstalledVoices.All.FirstOrDefault(v => v.Language == "de-DE" && v.Gender == VoiceGender.Male);
            sync.SetVoice(voice);

            // auf Ende des Vorlesevorganges warten
            await sync.SpeakTextAsync(TextToRead.Text);
        }
    }
}