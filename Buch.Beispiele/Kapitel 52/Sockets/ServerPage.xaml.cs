using System;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace TcpCommunication
{
    public partial class ServerPage
    {
        private const string Port = "4832";
        private StreamSocketListener _listener;

        public ServerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DisplayIPInformation();
        }

        private void DisplayIPInformation()
        {
            var ipInformation = new StringBuilder();

            // alle Netzwerkverbindungen durchlaufen
            foreach (var hostName in NetworkInformation.GetHostNames())
            {
                // Wenn Netzwerkverbindung aktiv ist, dann zur Ausgabe hinzufügen
                if (hostName.IPInformation != null)
                {
                    ipInformation.AppendLine(hostName.DisplayName);
                }
            }
            IpAddress.Text = ipInformation.ToString();
        }

        private async void StartListing_Click(object sender, RoutedEventArgs e)
        {
            // Listener für Annahme von TCP-Verbindungsanfragen erzeugen
            _listener = new StreamSocketListener();

            // Eventhandler registrieren, um Verbindungsanfragen zu bearbeiten
            _listener.ConnectionReceived += Listener_ConnectionReceived;

            // Lauschen und warten auf Verbindungen starten
            Status.Text = "Starte Listener...";
            await _listener.BindServiceNameAsync(Port);
            Status.Text = "Listener bereit.";
        }
        
        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // Statustext aktualisieren
            UpdateStatus(string.Format("Verbunden mit {0}...", args.Socket.Information.RemoteHostName));

            // Bild übertragen
            TransferPicture(args.Socket);
        }

        private async void TransferPicture(StreamSocket socket)
        {
            // DataWriter erzeugen, um Byte-Umwandlung erledigen zu lassen...
            var writer = new DataWriter(socket.OutputStream);

            // Anzahl der zu übertragenden Bytes übertragen
            writer.WriteInt32(App.PhotoBytesToShare.Length);
            await writer.StoreAsync();

            // Image-Bytes übertragen
            writer.WriteBytes(App.PhotoBytesToShare);
            await writer.StoreAsync();
            await writer.FlushAsync();
            UpdateStatus("Übertragung abgeschlossen.");

            // Ressourcen freigeben
            writer.Dispose();
            socket.Dispose();

            // Beenden der Annahme von Client-Verbindungen
            _listener.Dispose();
        }

        private void UpdateStatus(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => Status.Text = message);
        }
    }
}