using Microsoft.Phone.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Aktienkurse.Views
{
    public partial class MainView : PhoneApplicationPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Senden der Message die das Laden der Aktienliste im ViewModel aus löst
            Messenger.Default.Send<object>(null, "UpdateListMessage");
        }
    }
}