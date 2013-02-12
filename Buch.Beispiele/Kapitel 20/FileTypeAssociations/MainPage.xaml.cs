using System;
using System.Windows.Input;
using FileAssociations.Models;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace FileAssociations
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void OnLoadingFile(object sender, GestureEventArgs e)
        {
            var city = new City
            {
                Name = "Leipzig",
                State = "Saxony",
                Country = "Germany",
                Population = 550000
            };

            string json = JsonConvert.SerializeObject(city);

            var cityFile = 
              await ApplicationData.Current
                                   .LocalFolder.CreateFileAsync(
                                     "leipzig.city",
                                     CreationCollisionOption.ReplaceExisting);

            using (var stream = await cityFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var writer = new DataWriter(stream))
                {
                    writer.WriteString(json);
                    await writer.StoreAsync();
                }
            }

            Launcher.LaunchFileAsync(cityFile);
        }
    }
}