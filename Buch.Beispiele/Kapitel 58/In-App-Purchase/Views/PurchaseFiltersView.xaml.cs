using InAppPurchase.ViewModels;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace InAppPurchase.Views
{
    public partial class PurchaseFiltersView : PhoneApplicationPage
    {
        public PurchaseFiltersView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ((PurchaseFilters)DataContext).LoadFilters();
        }
    }
}