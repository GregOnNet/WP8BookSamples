using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace NfcCommunikation
{
    public partial class ServerPage
    {
        public ServerPage()
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
                TransferImage(args.Socket);
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

        private async void TransferImage(StreamSocket socket)
        {
            var writer = new DataWriter(socket.OutputStream);

            UpdateStatus("Übertrage Bild...");

            // Anzahl der zu übertragenden Bytes übertragen
            writer.WriteInt32(App.ImageBytesToTransfer.Length);
            await writer.StoreAsync();

            // Image-Bytes übertragen
            writer.WriteBytes(App.ImageBytesToTransfer);
            await writer.StoreAsync();
            await writer.FlushAsync();

            // Ressourcen freigeben
            writer.Dispose();
            UpdateStatus("Übertragung abgeschlossen");
        }
    }
}