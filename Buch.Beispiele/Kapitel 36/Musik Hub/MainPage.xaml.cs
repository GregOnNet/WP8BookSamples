using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace MusikHubApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Titel, der gerade wiedergegeben wird
        Song playingSong;

        // Von Hub-Verlauf gestartet
        bool isFromHubHistory;

        // Key für MediaHistoryItem Key/Value
        const string playSongKey = "playSong";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Timer starten, um die XNA-Internas ausführen zu können (MediaPlayer ist vom XNA-Framework)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += (sender, e) =>
            {
                try
                {
                    FrameworkDispatcher.Update();
                }
                catch { }
            };

            // Media Player Buttons ein-/ausschalten in Abhängigkeit von Wiedergabe
            MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(MediaPlayer_MediaStateChanged);
        }

        #region Event Handlers

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            MediaLibrary library = new MediaLibrary();
            if (NavigationContext.QueryString.ContainsKey(playSongKey))
            {
                // Start über Hub-Verlauf
                // playingSong direkt übernehmen und starten
                string songName = NavigationContext.QueryString[playSongKey];
                playingSong = library.Songs.FirstOrDefault(s => s.Name == songName);
                isFromHubHistory = true;
            }
            else if (MediaPlayer.State == MediaState.Playing)
            {
                // Aktuellen Song übernehmen
                playingSong = MediaPlayer.Queue.ActiveSong;
            }
            else
            {
                // Zufälligen Song auswählen
                Random r = new Random();
                int songsCount = library.Songs.Count;
                if (songsCount > 0)
                {
                    playingSong = library.Songs[r.Next(songsCount)];
                }
                else
                {
                    SongName.Text = "Keine Songs gefunden";
                    Play.IsEnabled = false;
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Albumart und Songnamen setzen
            PopulateSongMetadata();

            // Buttons aktivieren/deaktiveren je nach STatus
            SetButtonState();

            if (isFromHubHistory)
            {
                // Von Hub-Verlaufeintrag gestartet => Wiedergabe sofort starten
                PlaySong();
            }
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            SetButtonState();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            PlaySong();
            AddToHistory();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopSong();
        }

        private void Search_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            SearchWikipedia();
            e.Complete();
        }

        #endregion

        #region Media Player

        private void PlaySong()
        {
            if (playingSong != null)
            {
                MediaPlayer.Play(playingSong);
            }
	    }

        private void StopSong()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }

        private void SetButtonState()
        {
            switch (MediaPlayer.State)
            {
                case MediaState.Playing:
                    Play.IsEnabled = false;
                    Stop.IsEnabled = true;
                    break;
                case MediaState.Paused:
                case MediaState.Stopped:
                    Play.IsEnabled = true;
                    Stop.IsEnabled = false;
                    break;
            }
        }

        private void PopulateSongMetadata()
        {
            if (playingSong == null)
            {
                return;
            }

            // Songtitel auslesen und setzen
            SongName.Text = playingSong.Name;

            // Versuchen das Albumcover auszulesen und zu setzen
            Stream albumArtStream = playingSong.Album.GetAlbumArt();
            if (albumArtStream != null)
            {
                BitmapImage albumArt = new BitmapImage();
                albumArt.SetSource(albumArtStream);
                SongAlbumArt.Source = albumArt;
            }
        }

        #endregion

        private void AddToHistory()
        {
            if (playingSong == null)
            {
                return;
            }

            MediaHistoryItem historyItem = new MediaHistoryItem();
            historyItem.Title = playingSong.Name;
            
            // TODO: Hier ein aussagekräftigeres Icon verwenden, dass dieser Verlaufseintrag von der App stammt
            historyItem.ImageStream = playingSong.Album.GetAlbumArt();

            // Wenn die App über den Verlauf gestartet wird, bekommen wir den Songnamen wieder geliefert
            historyItem.PlayerContext[playSongKey] = playingSong.Name;

            // Verlauf aktualisieren
            MediaHistory.Instance.WriteRecentPlay(historyItem);
        }

        private void SearchWikipedia()
        {
            if (playingSong != null)
            {
                // Interpetnamen abrufen und Leerzeichen umwandeln für Wikipedia
                string artistName = playingSong.Artist.Name.Replace(" ", "_");

                // URL-Kodierung durchführen
                string artist = Uri.EscapeDataString(artistName);

                // Finale URL bauen und WebbBrowser öffnen via Launcher
                string uri = "http://de.wikipedia.org/wiki/" + artist;
                WebBrowserTask browser = new WebBrowserTask();
                browser.Uri = new Uri(uri);
                browser.Show();
            }
        }  
    }
}