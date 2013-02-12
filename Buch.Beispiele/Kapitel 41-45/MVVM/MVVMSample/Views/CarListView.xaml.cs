using Microsoft.Phone.Controls;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace MVVMSample
{
    public partial class CarListView : PhoneApplicationPage
    {
        public CarListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CarListBox.SelectedIndex = -1;
        }
    }
}