using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using System.IO.IsolatedStorage;

namespace Twitter
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!e.IsNavigationInitiator || e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                if (App.WasAppDormant)
                {
                    // App wurde im Speicher gehalten => nichts machen
                    return;
                }

                if (this.State.ContainsKey("LastUser"))
                {
                    // App wurde reaktiviert
                    string lastUser = (string)this.State["LastUser"];
                    this.TwitterName.Text = lastUser;
                    this.DownloadTwitterTimeline(lastUser);
                }
                else
                {
                    // App wurde gestartet
                    string lastTwitterUser;
                    var c = IsolatedStorageSettings.ApplicationSettings;
                    if (c.Contains("LastTwitterUser"))
                    {
                        string user = (string)c["LastTwitterUser"];
                    }

                    
                    string t2;
                    if (c.TryGetValue("LastTwitterUser", out t2))
                    {
                        this.TwitterName.Text = t2;
                    }
                }

                // Nicht mehr benötigten Page-State löschen
                this.State.Remove("LastUser");
            }            
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            // Status sichern im Page-State
            this.State["LastUser"] = this.TwitterName.Text;

            // User-Eigabe persistieren
            var cfg = IsolatedStorageSettings.ApplicationSettings;
            cfg["LastTwitterUser"] = this.TwitterName.Text;
            cfg.Save();
        }

        private void LoadTimeline_Click(object sender, RoutedEventArgs e)
        {
            DownloadTwitterTimeline(this.TwitterName.Text);            
        }

        private void DownloadTwitterTimeline(string user)
        {
            // URL zur Twitter-Timelines des Users erstellen
            string timelineUrl = "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + user;

            // Mittels WebClient die Timeline als XML downloaden
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(timelineUrl));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // Wenn Fehler aufgetreten sind weitere Verarbeitung beenden
            if (e.Error != null)
            {
                return;
            }

            // Timeline in ein XElement überführen zum einfachen Parsen
            XElement xmlTweets = XElement.Parse(e.Result);

            // Timeline parsen und in eigenes Objekt überführen
            List<TwitterItem> timeline = (from tweet in xmlTweets.Descendants("status")
                                          select new TwitterItem
                                          {
                                              ImageUrl = tweet.Element("user").Element("profile_image_url").Value,
                                              Message = tweet.Element("text").Value,
                                              UserName = tweet.Element("user").Element("screen_name").Value
                                          }).ToList();

            // Timeline in ListBox anzeigen
            this.Timeline.ItemsSource = timeline;
        }

        private void Timeline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Selektierten Eintrag abrufen
            var selectedTweet = this.Timeline.SelectedItem as TwitterItem;
            
            if (selectedTweet != null)
            {
                // Wenn Cast erfolgreich war Eintrag speichern und zur nächsten Seite navigieren
                App.SelectedTweet = selectedTweet;
                this.NavigationService.Navigate(new Uri("/TweetDetails.xaml", UriKind.Relative));
            }
        }
    }
}