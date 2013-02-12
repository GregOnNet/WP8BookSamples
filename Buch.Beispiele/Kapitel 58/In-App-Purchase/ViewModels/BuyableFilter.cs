using System;
using System.IO.IsolatedStorage;
using System.Windows;
using InAppPurchase.Commands;

#if DEBUG
using MockIAPLib;
#else
using Windows.ApplicationModel.Store;
#endif

namespace InAppPurchase.ViewModels
{
    public class BuyableFilter : ViewModelBase
    {
        public BuyableFilter()
        {
            BuyCommand = new ActionCommand<object>(o => Buy());
        }

        private string _filterName;

        public string FilterName
        {
            get { return _filterName; }
            set
            {
                if (_filterName == value)
                    return;

                _filterName = value;
                SendPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                _description = value;
                SendPropertyChanged();
            }
        }

        private string _productId;

        public string ProductId
        {
            get { return _productId; }
            set
            {
                if (_productId == value)
                    return;

                _productId = value;
                SendPropertyChanged();
            }
        }

        private string _price;

        public string Price
        {
            get { return _price; }
            set
            {
                if (_price == value)
                    return;

                _price = value;
                SendPropertyChanged();
            }
        }

        private bool _isBought;

        public bool IsBought
        {
            get { return _isBought; }
            set
            {
                if (_isBought == value)
                    return;

                _isBought = value;
                SendPropertyChanged();
                SendPropertyChanged("BuyVisibility");
                SendPropertyChanged("BoughtVisibility");
            }
        }

        public Visibility BuyVisibility
        {
            get { return IsBought ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility BoughtVisibility
        {
            get { return IsBought ? Visibility.Visible : Visibility.Collapsed; }
        }

        private ActionCommand<object> _buyCommand;

        public ActionCommand<object> BuyCommand
        {
            get { return _buyCommand; }
            set
            {
                if (_buyCommand == value)
                    return;

                _buyCommand = value;
                SendPropertyChanged();
            }
        }

        public async void Buy()
        {
            string buyInfo = await CurrentApp.RequestProductPurchaseAsync(ProductId, false);
            if (CurrentApp.LicenseInformation.ProductLicenses.ContainsKey(ProductId) &&
                CurrentApp.LicenseInformation.ProductLicenses[ProductId].IsActive)
            {
                //CurrentApp.ReportProductFulfillment(ProductId);

                IsolatedStorageSettings.ApplicationSettings[ProductId] = true;
                IsBought = true;
            }
        }
    }
}
