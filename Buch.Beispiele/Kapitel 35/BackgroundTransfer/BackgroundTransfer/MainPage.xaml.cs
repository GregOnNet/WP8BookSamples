using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Microsoft.Phone.BackgroundTransfer;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace BackgroundTransfer
{
    public partial class MainPage
    {
        public ApplicationBarIconButton DownloadButton { get; private set; }
        public ApplicationBarIconButton DeleteButton { get; private set; }

        public string TransfersFolder
        {
            get { return "/shared/transfers"; }
        }

        public string DownloadLocation
        {
            get { return TransfersFolder + "/TheVideo.wmv"; }
        }

        private BackgroundTransferRequest[] _transferRequests;

        public MainPage()
        {
            InitializeComponent();

            DownloadButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            DeleteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
        }

        private void Download_Click(object sender, EventArgs e)
        {
            try
            {
                // Sichergehen, dass der Download-Ordner existiert
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isoStore.DirectoryExists(TransfersFolder))
                    {
                        isoStore.CreateDirectory(TransfersFolder);
                    }
                }

                var request = new BackgroundTransferRequest(new Uri("http://ralfe-software.net/Wildlife.wmv", UriKind.Absolute))
                    {
                        DownloadLocation = new Uri(DownloadLocation, UriKind.Relative),
                        Method = "GET",
                        TransferPreferences = TransferPreferences.AllowBattery
                    };
                BackgroundTransferService.Add(request);

                DownloadButton.IsEnabled = false;
                DeleteButton.IsEnabled = true;
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UpdateUI()
        {
            UpdateTransferRequests();

            if (_transferRequests == null || _transferRequests.Length == 0)
            {
                Status.Text = "Keine Transfers";
                DownloadProgress.Visibility = Visibility.Collapsed;

                DownloadButton.IsEnabled = true;
                DeleteButton.IsEnabled = false;
            }
            else
            {
                // Download-Anzeige anzeigen
                DownloadProgress.Visibility = Visibility.Visible;
                DownloadProgress.Value = 0;

                // Ereignisse registrieren
                _transferRequests[0].TransferStatusChanged += MainPage_TransferStatusChanged;
                _transferRequests[0].TransferProgressChanged += MainPage_TransferProgressChanged;

                // Transfer bearbeiten
                ProcessTransfer(_transferRequests[0]);
            }
        }

        private void UpdateTransferRequests()
        {
            // Bisherige Objekte freigeben
            if (_transferRequests != null)
            {
                foreach (var item in _transferRequests)
                {
                    // Event-Handler abmelden, sonst Memory Leak
                    item.TransferProgressChanged -= MainPage_TransferProgressChanged;
                    item.TransferStatusChanged -= MainPage_TransferStatusChanged;

                    // Objekt freigeben
                    item.Dispose();
                }
            }

            _transferRequests = BackgroundTransferService.Requests.ToArray();
        }

        private void ProcessTransfer(BackgroundTransferRequest request)
        {
            switch (request.TransferStatus)
            {
                case TransferStatus.Completed:
                    if (request.StatusCode == 200 || request.StatusCode == 206)
                    {
                        Status.Text = "Abgeschlossen";

                        // Download fertig - Transfer-Obhjekt entfernen
                        RemoveTransferRequest(request);

                        // AppBar-Buttons aktualisieren
                        DeleteButton.IsEnabled = true;
                        DownloadButton.IsEnabled = false;

                        // Video abspielen
                        string sourcePath = request.DownloadLocation.OriginalString.Substring(1);
                        var player = new MediaPlayerLauncher();
                        player.Location = MediaLocationType.Data;
                        player.Media = new Uri(sourcePath, UriKind.Relative);
                        player.Orientation = MediaPlayerOrientation.Landscape;
                        player.Show();
                    }
                    break;
                default:
                    Status.Text = request.TransferStatus.ToString();
                    DeleteButton.IsEnabled = true;
                    DownloadButton.IsEnabled = false;
                    break;
            }
        }

        private void RemoveTransferRequest(BackgroundTransferRequest request)
        {
            try
            {
                request.TransferProgressChanged -= MainPage_TransferProgressChanged;
                request.TransferStatusChanged -= MainPage_TransferStatusChanged;
                BackgroundTransferService.Remove(request);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MainPage_TransferProgressChanged(object sender, BackgroundTransferEventArgs e)
        {
            double downloadProgress = (e.Request.BytesReceived * 100.0) / e.Request.TotalBytesToReceive;
            DownloadProgress.Value = downloadProgress;
        }

        private void MainPage_TransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            UpdateUI();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists(DownloadLocation))
                {
                    isoStore.DeleteFile(DownloadLocation);
                }
            }

            if (_transferRequests != null && _transferRequests.Length == 0)
            {
                var request = _transferRequests[0];
                RemoveTransferRequest(request);
            }

            UpdateUI();
        }
    }
}