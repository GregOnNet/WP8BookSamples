using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace BluetoothCommunication
{
    public partial class ClientPage
    {
        public ClientPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StartWaitingForPeer();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Keine weiteren Peers mehr annehmen
            PeerFinder.Stop();
            base.OnNavigatingFrom(e);
        }

        private void StartWaitingForPeer()
        {
            // Auf Peerverbindung warten
            PeerFinder.Start();
            PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;
        }

        private void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            // Zurück auf UI Thread wechseln
            Dispatcher.BeginInvoke(() => AskUserToAcceptPeerRequest(args.PeerInformation));
        }

        private void AskUserToAcceptPeerRequest(PeerInformation peer)
        {
            var result = MessageBox.Show(string.Format("Mit Peer {0} verbinden?", peer.DisplayName),
                                         "BLUETOOTH KOMMUNIKATION", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ReceiveImage(peer);
            }
        }

        private async void ReceiveImage(PeerInformation peer)
        {
            try
            {
                Status.Text = "Verbinde mit Peer...";
                StreamSocket peerSocket = await PeerFinder.ConnectAsync(peer);
                Status.Text = "Verbunden. Empfange Daten...";

                // DataReader erzeugen, um arbeiten mit Bytes zu vereinfachen
                var reader = new DataReader(peerSocket.InputStream);

                // Anzahl der Bytes abrufen, aus denen das Bild besteht
                // Anzahl = int = 4 Bytes => 4 Bytes vom Socket laden
                await reader.LoadAsync(4);
                int imageSize = reader.ReadInt32();

                // Bytearray des Bildes laden
                await reader.LoadAsync((uint)imageSize);
                byte[] imageBytes = new byte[imageSize];
                reader.ReadBytes(imageBytes);

                // Bytearray in Stream laden und anzeigen
                using (var ms = new MemoryStream(imageBytes))
                {
                    var image = new BitmapImage();
                    image.SetSource(ms);

                    ReceivedImage.Source = image;
                }
                Status.Text = "Bild empfangen.";

                // Ressourcen freigeben
                reader.Dispose();
                peerSocket.Dispose();

                // Wieder Verbindungen akzeptieren
                PeerFinder.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
                Status.Text = "Bereit.";
            }
        }
    }
}