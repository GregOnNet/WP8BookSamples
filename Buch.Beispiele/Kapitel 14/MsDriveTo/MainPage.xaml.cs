using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace MsDriveTo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const double TargetLatitude = 52.506389;
        private const double TargetLongitude = 13.377022;
        private const string TargetName = "Fernsehturm, Berlin";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void MsWalkTo_Click(object sender, RoutedEventArgs e)
        {
            var navUri =
                string.Format("ms-walk-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                              TargetLatitude, TargetLongitude, TargetName);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(navUri));
        }

        private async void MsDriveTo_Click(object sender, RoutedEventArgs e)
        {
            var navUri =
                string.Format("ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                              TargetLatitude, TargetLongitude, TargetName);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(navUri));
        }
    }
}