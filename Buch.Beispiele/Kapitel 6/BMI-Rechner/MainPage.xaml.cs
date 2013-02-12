using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;

namespace BmiRechner
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
        }

        private void GoToResults(object sender, RoutedEventArgs e)
        {
            string height = this.HeightInput.Text;
            string weight = this.WeightInput.Text;

            // Ziel-Uri bauen  - notwendiges Dictionary wird mit Collection-Initializers gefüllt
            Uri pageUri = NavigationHelper.BuildUri("/Result.xaml", new Dictionary<string, object> { { "Height", height }, { "Weight", weight } });
            this.NavigationService.Navigate(pageUri);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                // Seitenstatus sichern
                SavePageState();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Seitenstatus laden (soweit vorhanden)
            this.LoadPageState();
        }

        private void SavePageState()
        {
            this.State["Height"] = this.HeightInput.Text;
            this.State["Weight"] = this.WeightInput.Text;            
        }

        private void LoadPageState()
        {
            if (this.State.ContainsKey("Height"))
            {
                this.HeightInput.Text = (string)this.State["Height"];
                this.State.Remove("Height");
            }

            object weight;
            if (this.State.TryGetValue("Weight", out weight))
            {
                this.WeightInput.Text = (string)weight;
                this.State.Remove("Weight");
            }
        }
    }
}