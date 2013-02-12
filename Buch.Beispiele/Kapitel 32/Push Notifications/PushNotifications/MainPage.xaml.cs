using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Shell;
using System.Text;
using System.IO;

namespace PushNotifications
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainPageLoaded);
        }

        private void MainPageLoaded(object sender,
                                    RoutedEventArgs e)
        {
            HttpNotificationChannel pushChannel;
            string channelName = "NotificationSampleChannel";

            pushChannel =
                HttpNotificationChannel
                .Find(channelName);

            if (pushChannel == null)
            {
                pushChannel =
                    new HttpNotificationChannel
                                  (channelName);
                pushChannel.ChannelUriUpdated +=
                    new EventHandler
                        <NotificationChannelUriEventArgs>
                        (PushChannelChannelUriUpdated);

                pushChannel.ErrorOccurred +=
                    new EventHandler
                        <NotificationChannelErrorEventArgs>
                        (PushChannelErrorOccurred);

                pushChannel.ShellToastNotificationReceived +=
                    new EventHandler<NotificationEventArgs>
                        (pushChannel_ShellToastNotificationReceived);

                pushChannel.HttpNotificationReceived +=
                    new EventHandler<HttpNotificationEventArgs>
                    (pushChannel_HttpNotificationReceived);

                pushChannel.Open();
                pushChannel.BindToShellTile();
                pushChannel.BindToShellToast();
            }
            else
            {
                pushChannel.ChannelUriUpdated +=
                    new EventHandler
                        <NotificationChannelUriEventArgs>
                    (PushChannelChannelUriUpdated);
                pushChannel.ErrorOccurred +=
                    new EventHandler
                        <NotificationChannelErrorEventArgs>
                        (PushChannelErrorOccurred);

                pushChannel.ShellToastNotificationReceived +=
                    new EventHandler<NotificationEventArgs>
                        (pushChannel_ShellToastNotificationReceived);

                pushChannel.HttpNotificationReceived +=
                    new EventHandler<HttpNotificationEventArgs>
                        (pushChannel_HttpNotificationReceived);

                Debug.WriteLine
                    (pushChannel.ChannelUri
                                .ToString());
                MessageBox.Show
                    (pushChannel.ChannelUri
                                .ToString());
            }
        }

        void pushChannel_HttpNotificationReceived
            (object sender,
            HttpNotificationEventArgs e)
        {
            string message;

            using (StreamReader reader = new StreamReader
                (e.Notification.Body))
            {
                message = reader.ReadToEnd();
            }
            
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(
                    String.Format
                    ("Empfangene Nachricht: {0}\n{1}",
                    DateTime.Now.ToShortTimeString(),
                    message))
                    );
        }

        void pushChannel_ShellToastNotificationReceived
            (object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat
                ("Empfangene Nachricht: {0}\n",
                DateTime.Now.ToShortTimeString());

            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat("{0}\n",
                   e.Collection[key]);
            }

            Dispatcher.BeginInvoke(
                () => MessageBox.Show(message.ToString()));
        }

        private void SetStandardTile()
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();

            if (appTile != null)
            {
                StandardTileData standardTileData =
                    new StandardTileData
                {
                    Title = "Push Notifications",
                    BackgroundImage =
                        new Uri("/Background.png",
                        UriKind.Relative),
                    Count = 0,
                    BackTitle = "",
                    BackBackgroundImage =
                        new Uri("",
                        UriKind.Relative),
                    BackContent = ""
                };

                appTile.Update(standardTileData);
            }
        }

        void PushChannelErrorOccurred
            (object sender,
            NotificationChannelErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(
                String.Format(
                @"Ein Push- Notification {0} 
                  Fehler ist aufgretreten.
                 {1} {2} {3}",
                e.ErrorType, e.ErrorCode,
                e.ErrorAdditionalData))
                );
        }

        void PushChannelChannelUriUpdated
            (object sender,
             NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Debug.WriteLine(e.ChannelUri.ToString());
                MessageBox.Show(String.Format
                    ("Die Kanal- Uri lautet {0}",
                    e.ChannelUri.ToString()));
            });
        }
    }
}