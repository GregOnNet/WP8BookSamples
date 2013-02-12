using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Wallet;
using System.IO;
using System.Windows.Media.Imaging;

namespace WalletExample
{
    public partial class DealsPage : PhoneApplicationPage
    {
        public DealsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadDealsFromWallet();
        }

        private async void LoadDealsFromWallet()
        {
            WalletItemCollection allItems = await Wallet.GetItemsAsync();
            var deals = allItems.Where(wi => wi is Deal).ToList();
            Deals.ItemsSource = deals;
        }

        private async void SaveDeal_Click(object sender, RoutedEventArgs e)
        {
            var deal = new Deal(Guid.NewGuid().ToString("N"));
            deal.IssuerName = "Fake Wein Club";
            deal.IsUsed = false;
            deal.MerchantName = "Fake Wein Club";
            deal.TermsAndConditions = "Pro Tag und Person ist ein Deal einlösbar.";
            deal.ExpirationDate = DateTime.Now.AddDays(14);
            deal.DisplayName = "10€ Nachlass";
            deal.Description = "Sie erhalten auf Ihren nächsten Einkauf 10€ erlassen.";
            deal.Code = "QRNachlass10";

            var qrCode = new BitmapImage();
            using (Stream logoStream = Application.GetResourceStream(new Uri("Assets/QRNachlass.png", UriKind.Relative)).Stream)
            {
                qrCode.SetSource(logoStream);
            }
            deal.BarcodeImage = qrCode;

            try
            {
                SystemTray.ProgressIndicator.IsVisible = true;
                await deal.SaveAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es gab ein Problem beim Speichern: " + ex.Message);
            }
            finally
            {
                SystemTray.ProgressIndicator.IsVisible = false;
            }

            LoadDealsFromWallet();
        }

        private async void MarkAsUsed_Click(object sender, RoutedEventArgs e)
        {
            // gebundenen Deal abrufen
            var element = sender as FrameworkElement;
            var deal = (Deal)element.DataContext;

            // Deals als benutzt markieren und speichern
            deal.IsUsed = true;
            await deal.SaveAsync();

            // Deals neu laden
            LoadDealsFromWallet();
        }

        private void RemoveFromWallet_Click(object sender, RoutedEventArgs e)
        {
            // gebundenen Deal abrufen
            var element = sender as FrameworkElement;
            var deal = (Deal)element.DataContext;

            // Deals aus Wallet entfernen
            bool success = Wallet.Remove(deal);

            // Deals neu laden
            LoadDealsFromWallet();
        }
    }
}