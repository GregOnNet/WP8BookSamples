using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;

namespace BackgroundMusik
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;
        }

        private void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    playButton.Content = "Pause";
                    break;

                case PlayState.Paused:
                case PlayState.Stopped:
                    playButton.Content = "Play";
                    break;
            }

            if (BackgroundAudioPlayer.Instance.Track != null)
            {
                currentTrack.Text = string.Format("{0} ({1})", BackgroundAudioPlayer.Instance.Track.Title, BackgroundAudioPlayer.Instance.Track.Artist);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                playButton.Content = "Pause";
                currentTrack.Text = string.Format("{0} ({1})", BackgroundAudioPlayer.Instance.Track.Title, BackgroundAudioPlayer.Instance.Track.Artist);
            }
            else
            {
                playButton.Content = "Play";
                currentTrack.Text = "";
            }
        }

        #region Button Click Event Handlers

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.SkipPrevious();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                BackgroundAudioPlayer.Instance.Pause();
            }
            else
            {
                BackgroundAudioPlayer.Instance.Play();
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.SkipNext();
        }

        #endregion Button Click Event Handlers        
    }
}