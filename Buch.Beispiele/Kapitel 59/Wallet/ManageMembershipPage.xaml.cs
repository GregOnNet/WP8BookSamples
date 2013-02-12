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

namespace WalletExample
{
    public partial class ManageMembershipPage : PhoneApplicationPage
    {
        public ManageMembershipPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadMembership();
        }

        private async void LoadMembership()
        {
            var membership = Wallet.FindItem(App.MembershipWalletKey) as WalletTransactionItem;
            if (membership == null)
            {
                return;
            }

            FullName.Text = membership.CustomerName;
            MembershipNumber.Text = membership.AccountNumber;
            Email.Text = membership.CustomProperties[App.EmailWalletKey].Value;
            PhoneNumber.Text = membership.CustomProperties[App.PhoneWalletKey].Value;
            MemberSince.Text = membership.MemberSince.HasValue ? membership.MemberSince.Value.ToString("d") : string.Empty;
            Balance.Text = membership.DisplayBalance;

            membership.Notes = string.Format("Letzter Zugriff: {0}", DateTime.Now.ToLongTimeString());
            await membership.SaveAsync();
        }

        private void DeleteMembership_Click(object sender, RoutedEventArgs e)
        {
            var messageResult = MessageBox.Show("Möchten Sie wirklich Ihre Mitgliedschaft kündigen?", "Fake Wein Club", MessageBoxButton.OKCancel);
            if (messageResult != MessageBoxResult.OK)
            {
                return;
            }

            var successful = Wallet.Remove(App.MembershipWalletKey);
            if (successful)
            {
                MessageBox.Show("Ihre Mitgliedschaft wurde erfolgreich beendet.");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Es gab ein Problem Ihre Mitgliedschaft zu beenden. Versuchen Sie es erneut.");
            }
        }
    }
}