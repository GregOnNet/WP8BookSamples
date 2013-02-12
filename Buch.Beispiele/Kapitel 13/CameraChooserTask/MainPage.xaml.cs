using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace CameraChooserTaskApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
        }

        private void CameraButtonClick(object sender,
                                      RoutedEventArgs e)
        {
            CameraCaptureTask camera = new CameraCaptureTask();
            camera.Completed += new EventHandler
                                     <PhotoResult>(camera_Result);

            camera.Show();
        }

        void camera_Result(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);

                img_CapturedPhoto.Source = image;
            }
            else
            {
                MessageBox.Show("Ein Fehler ist aufgetreten");
            }
        }
    }
}