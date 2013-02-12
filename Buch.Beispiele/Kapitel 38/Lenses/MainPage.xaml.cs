using System;
using System.Windows;
using System.Windows.Navigation;
using BlackWhiteImageStyler.PhotoCapture;
using Microsoft.Phone.Shell;
using Windows.Phone.Media.Capture;
using Microsoft.Devices;

namespace BlackWhiteImageStyler
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SystemTray.ProgressIndicator.IsVisible = true;

            App.Camera.Focused += Camera_Focused;
            App.Camera.PhotoCaptured += Camera_PhotoCaptured;

            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            
            await App.Camera.InitializeAsync();
            CameraBrush.SetSource(App.Camera.Device);

            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            SystemTray.ProgressIndicator.IsVisible = false;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            App.Camera.Focused -= Camera_Focused;
            App.Camera.PhotoCaptured -= Camera_PhotoCaptured;

            base.OnNavigatingFrom(e);
        }

        private void Camera_Focused(object sender, FocusedEventArgs e)
        {
            FocusIndicator.Visibility = e.State == CameraFocusStatus.Locked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Camera_PhotoCaptured(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PhotoCapturedPage.xaml", UriKind.Relative));
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CameraSettingsPage.xaml", UriKind.Relative));
        }
    }
}