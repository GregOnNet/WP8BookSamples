using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace BluetoothCommunication
{
    public partial class MainPage
    {
        private readonly PhotoChooserTask _photoChooser;

        public MainPage()
        {
            InitializeComponent();

            _photoChooser = new PhotoChooserTask
            {
                ShowCamera = true,
                PixelWidth = 1280,
                PixelHeight = 720
            };
            _photoChooser.Completed += PhotoChooser_Completed;
        }

        private void PhotoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK)
                return;

            var image = new BitmapImage();
            image.SetSource(e.ChosenPhoto);

            var bitmap = new WriteableBitmap(image);
            using (var ms = new MemoryStream())
            {
                bitmap.SaveJpeg(ms, 1280, 720, 0, 85);
                App.ImageBytesToTransfer = ms.ToArray();
            }
            ImageToTransfer.Source = image;
        }

        private void ChoosePicture_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            _photoChooser.Show();
        }
    }
}