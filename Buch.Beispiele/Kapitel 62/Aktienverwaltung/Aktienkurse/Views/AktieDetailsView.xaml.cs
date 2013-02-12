using System;
using Microsoft.Phone.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Aktienkurse.Views
{
    public partial class AktieDetailsView : PhoneApplicationPage
    {
        public AktieDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("symbol") &&
                !String.IsNullOrEmpty(NavigationContext.QueryString["symbol"]))
            {
                //Nachriht, die das Laden von AktienDetails im ViewModel auslöst
                Messenger.Default.Send<string>(NavigationContext.QueryString["symbol"], "LoadAktieDetailsBySymbolMessage");
            }
        }
    }
}