using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace BlackWhiteImageStyler
{
    public partial class PhotoCapturedPage
    {
        private readonly MediaLibrary _library;

        public PhotoCapturedPage()
        {
            InitializeComponent();
            _library = new MediaLibrary();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.Camera.ImageBytes == null)
            {
                return;
            }

            using (var ms = new MemoryStream(App.Camera.ImageBytes))
            {
                var image = new BitmapImage();
                image.SetSource(ms);
                CapturedImage.Source = image;   
            }
        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            _library.SavePictureToCameraRoll("Demo.jpg", App.Camera.ImageBytes);
        }
    }
}