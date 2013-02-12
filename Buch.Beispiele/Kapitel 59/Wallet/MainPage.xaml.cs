using Microsoft.Phone.Controls;
using Microsoft.Phone.Wallet;
using System.Windows;
using System.Windows.Navigation;

namespace WalletExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsMembershipCardInWallet())
            {
                BecomeMember.Visibility = Visibility.Collapsed;
                ManageMembership.Visibility = Visibility.Visible;
            }
            else
            {
                BecomeMember.Visibility = Visibility.Visible;
                ManageMembership.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsMembershipCardInWallet()
        {
            var membershipItem = Wallet.FindItem(App.MembershipWalletKey) as WalletTransactionItem;
            return membershipItem != null;
        }
    }
}