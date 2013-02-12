using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Windows.Networking.Proximity;
using Windows.Storage.Streams;
using Windows.System;

namespace BluetoothCommunication
{
    public partial class ServerPage
    {
        private const uint BluetoothOff = 0x8007048F;

        public ServerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SearchForPeers();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Das Suchen nach Peers beenden
            PeerFinder.Stop();

            base.OnNavigatingFrom(e);
        }

        private async void SearchForPeers()
        {
            try
            {
                SystemTray.ProgressIndicator.IsVisible = true;
                
                // PeerFinder starten, um nach Peers suchen zu können
                PeerFinder.Start();

                // Nur nach verbundene Geräte suchen
                // PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";

                // Peers suchen und Ergebnis anzeigen
                IReadOnlyList<PeerInformation> foundPeers = await PeerFinder.FindAllPeersAsync();
                PeersList.ItemsSource = foundPeers;

                SystemTray.ProgressIndicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == BluetoothOff)
                {
                    // Bluetooth ist deaktiviert - Nutzer zu den Einstellungen navigieren zum Einschalten
                    Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void TransferPicture_Click(object sender, RoutedEventArgs e)
        {
            var selectedPeer = PeersList.SelectedItem as PeerInformation;
            if (selectedPeer == null)
            {
                MessageBox.Show("Bitte Emfängergerät wählen");
                return;
            }

            try
            {
                Status.Text = "Verbinde mit Peer...";
                var peerSocket = await PeerFinder.ConnectAsync(selectedPeer);
                var writer = new DataWriter(peerSocket.OutputStream);

                Status.Text = "Verbunden. Übertrage Bild...";

                // Anzahl der zu übertragenden Bytes übertragen
                writer.WriteInt32(App.ImageBytesToTransfer.Length);
                await writer.StoreAsync();

                // Image-Bytes übertragen
                writer.WriteBytes(App.ImageBytesToTransfer);
                await writer.StoreAsync();
                await writer.FlushAsync();

                // Ressourcen freigeben
                writer.Dispose();
                peerSocket.Dispose();
                Status.Text = "Übertragung abgeschlossen";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
                Status.Text = "Bereit.";
            }
        }
    }
}