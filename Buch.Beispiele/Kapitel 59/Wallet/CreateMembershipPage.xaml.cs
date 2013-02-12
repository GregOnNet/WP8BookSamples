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
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Tasks;

namespace WalletExample
{
    public partial class CreateMembershipPage : PhoneApplicationPage
    {
        private AddWalletItemTask _addWalletTask;

        public CreateMembershipPage()
        {
            InitializeComponent();

            _addWalletTask = new AddWalletItemTask();
            _addWalletTask.Completed += AddWalletTask_Completed;
        }
        
        private void CreateMembership_Click(object sender, RoutedEventArgs e)
        {
            var membershipItem = new WalletTransactionItem(App.MembershipWalletKey);
            membershipItem.IssuerName = "Fake Wein Club";
            membershipItem.DisplayName = "Fake Wein Club";
            membershipItem.CustomerName = string.Format("{0} {1}", FirstName.Text, LastName.Text);
            membershipItem.AccountNumber = GenerateMembershipNumber();
            membershipItem.CustomProperties.Add(App.EmailWalletKey, new CustomWalletProperty(Email.Text));
            membershipItem.CustomProperties.Add(App.PhoneWalletKey, new CustomWalletProperty(PhoneNumber.Text));
            membershipItem.DisplayBalance = "50 Treuepunkte";
            membershipItem.MemberSince = DateTime.Now;
            membershipItem.Message = "Ihre Mitglieds- und Treuekarte";

            var logo336 = new BitmapImage();
            using (Stream logoStream = Application.GetResourceStream(new Uri("Assets/Card336.jpg", UriKind.Relative)).Stream)
            {
                logo336.SetSource(logoStream);
            }

            var logo159 = new BitmapImage();
            using (Stream logoStream = Application.GetResourceStream(new Uri("Assets/Card159.jpg", UriKind.Relative)).Stream)
            {
                logo159.SetSource(logoStream);
            }

            var logo99 = new BitmapImage();
            using (Stream logoStream = Application.GetResourceStream(new Uri("Assets/Card99.jpg", UriKind.Relative)).Stream)
            {
                logo99.SetSource(logoStream);
            }

            membershipItem.Logo336x336 = logo336;
            membershipItem.Logo159x159 = logo159;
            membershipItem.Logo99x99 = logo99;

            _addWalletTask.Item = membershipItem;
            _addWalletTask.Show();
        }

        private string GenerateMembershipNumber()
        {
            var randomGenerator = new Random();
            int prefix = randomGenerator.Next(100, 1000);
            DateTime now = DateTime.Now;

            return string.Format("{0}-{1}{2}{3}", prefix, now.Year, now.Month, now.Day);
        }

        private void AddWalletTask_Completed(object sender, AddWalletItemResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show("Mitgliedschaftskarte wurde erfolgreich gespeichert :)");
                NavigationService.GoBack();
            }
            else if (e.TaskResult == TaskResult.Cancel)
            {
                MessageBox.Show("Vorgang wurde abgebrochen :(");
            }
            else
            {
                MessageBox.Show("Kein Ergebnis bekannt.");
            }
        }
    }
}