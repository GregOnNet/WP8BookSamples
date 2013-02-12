using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace NfcCommunikation
{
    public partial class ClientPage
    {
        public ClientPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WaitForConnection();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            EndConnection();
        }

        private void WaitForConnection()
        {
            PeerFinder.TriggeredConnectionStateChanged += PeerFinder_TriggeredConnectionStateChanged;
            PeerFinder.Start();
        }

        private void EndConnection()
        {
            PeerFinder.TriggeredConnectionStateChanged -= PeerFinder_TriggeredConnectionStateChanged;
            PeerFinder.Stop();
        }

        private void PeerFinder_TriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs args)
        {
            if (args.State == TriggeredConnectState.PeerFound)
            {
                UpdateStatus("Gerät gefunden! Halte die Geräte noch zusammen, bis die Verbindung aufgebaut wurde.");
            }

            if (args.State == TriggeredConnectState.Completed)
            {
                UpdateStatus("Verbunden!");
                ReceiveImage(args.Socket);
            }

            if (args.State == TriggeredConnectState.Failed || args.State == TriggeredConnectState.Canceled)
            {
                UpdateStatus("Verbindung fehlgeschlagen. Versuche es erneut.");
            }
        }

        private void UpdateStatus(string message)
        {
            Dispatcher.BeginInvoke(() => Status.Text = message);
        }

        private async void ReceiveImage(StreamSocket socket)
        {
            UpdateStatus("Empfange Daten...");

            // DataReader erzeugen, um arbeiten mit Bytes zu vereinfachen
            var reader = new DataReader(socket.InputStream);

            // Anzahl der Bytes abrufen, aus denen das Bild besteht
            // Anzahl = int = 4 Bytes => 4 Bytes vom Socket laden
            await reader.LoadAsync(4);
            int imageSize = reader.ReadInt32();

            // Bytearray des Bildes laden
            await reader.LoadAsync((uint)imageSize);
            byte[] imageBytes = new byte[imageSize];
            reader.ReadBytes(imageBytes);

            // Bytearray in Stream laden und anzeigen
            Dispatcher.BeginInvoke(() =>
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    var image = new BitmapImage();
                    image.SetSource(ms);

                    ReceivedImage.Source = image;
                }
            });
            UpdateStatus("Bild empfangen.");

            // Ressourcen freigeben
            reader.Dispose();
        }
    }
}