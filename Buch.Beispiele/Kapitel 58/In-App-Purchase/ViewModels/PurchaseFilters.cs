using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
#if DEBUG
using MockIAPLib;
#else
using Windows.ApplicationModel.Store;
#endif

namespace InAppPurchase.ViewModels
{
    public class PurchaseFilters : ViewModelBase
    {
        public PurchaseFilters()
        {
            if (IsInDesigner)
            {
                CreateDesignData();
            }
        }

        private void CreateDesignData()
        {
            var brightness = new BuyableFilter
            {
                FilterName = "Helligkeit",
                Description = "Ändere die Bild-Helligkeit",
                IsBought = true,
                Price = 0.99m.ToString("C")
            };
            var contrast = new BuyableFilter
            {
                FilterName = "Kontrast",
                Description = "Ändere den Bild-Kontrast",
                IsBought = false,
                Price = 0.99m.ToString("C")
            };
            Filters = new ObservableCollection<BuyableFilter> { brightness, contrast };
        }

        private Visibility _isLoading;

        public Visibility IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;
                SendPropertyChanged();
            }
        }

        public async void LoadFilters()
        {
            IsLoading = Visibility.Visible;

            var productInfo = await CurrentApp.LoadListingInformationAsync();
            Filters = new ObservableCollection<BuyableFilter>(
                from p in productInfo.ProductListings
                select new BuyableFilter
                {
                    FilterName = p.Value.Name,
                    ProductId = p.Value.ProductId,
                    Description = p.Value.Description,
                    IsBought = IsolatedStorageSettings.ApplicationSettings.Contains(p.Value.ProductId),
                    Price = p.Value.FormattedPrice
                }
                );

            IsLoading = Visibility.Collapsed;
        }

        private ObservableCollection<BuyableFilter> _filters;

        public ObservableCollection<BuyableFilter> Filters
        {
            get { return _filters; }
            set
            {
                if (_filters == value)
                    return;

                _filters = value;
                SendPropertyChanged();
            }
        }

    }
}
