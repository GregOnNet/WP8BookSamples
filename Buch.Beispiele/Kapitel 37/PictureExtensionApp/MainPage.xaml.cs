using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using System.IO;

namespace PictureExtensionApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string fromChooser = "FromChooser";
        private MediaLibrary library;
        private PhotoChooserTask photoChooser;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            library = new MediaLibrary();
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += photoChooser_Completed;
        }

        private void photoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.ChosenPhoto != null)
            {
                BitmapImage image = new BitmapImage();
                image.CreateOptions = BitmapCreateOptions.None;
                image.SetSource(e.ChosenPhoto);
                TheImage.Source = image;
            }
        }

        private void MakeGrayScale_Click(object sender, RoutedEventArgs e)
        {
            TheImage.Source = MakeGray(TheImage);
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            SavePicture();
        }
      
        private WriteableBitmap MakeGray(Image img)
        {
            WriteableBitmap bitmap = new WriteableBitmap(img, null);
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    // Das x. Pixel in der y. Zeile holen
                    int pixelLocation = bitmap.PixelWidth * y + x;
                    int pixel = bitmap.Pixels[pixelLocation];

                    // Pixel-Bytes holen (Format: ARGB)
                    byte[] pixelBytes = BitConverter.GetBytes(pixel);

                    // Grauwert = 0.30*A + 0.59*G + 0.11*B
                    byte bwPixel = (byte)(0.30 * pixelBytes[2] + 0.59 * pixelBytes[1] + 0.11 * pixelBytes[0]);
                    pixelBytes[0] = bwPixel;
                    pixelBytes[1] = bwPixel;
                    pixelBytes[2] = bwPixel;

                    // Neuen Pixelfarbwert setzen
                    bitmap.Pixels[pixelLocation] = BitConverter.ToInt32(pixelBytes, 0);
                }
            }
            return bitmap;
        }

        private void SavePicture()
        {
            // Das S/W-Bild kodieren, damit es gespeichert werden kann
            WriteableBitmap bitmap = new WriteableBitmap(TheImage, null);
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.SaveJpeg(ms, bitmap.PixelWidth, bitmap.PixelHeight, 0, 85);

                // Zurück zum Anfang des Stream gehen
                ms.Position = 0;

                // Enkodiertes Bild speichern
                library.SavePicture("PictureSW.jpg", ms);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // QueryString Date abrufen
            IDictionary<string, string> queryStrings = NavigationContext.QueryString;

            if (queryStrings.ContainsKey("Action"))
            {
                string fileToken = string.Empty;
                if (queryStrings.ContainsKey("token"))
                {
                    fileToken = queryStrings["token"];
                }
                else if (queryStrings.ContainsKey("FileId"))
                {
                    fileToken = queryStrings["FileId"];
                }

                if (!string.IsNullOrEmpty(fileToken))
                {
                    // Gestartet von Bilder-App => Bild laden
                    Picture picture = library.GetPictureFromToken(fileToken);
                    TheImage.Source = GetImage(picture);
                }
                
                // Action als Seitentitel verwenden
                PageTitle.Text = queryStrings["Action"];
            }
            else
            {
                if (State.ContainsKey(fromChooser))
                {
                    // Zurückgekehrt von Chooser - nicht erneut anzeigen
                    State.Remove(fromChooser);
                }
                else
                {
                    // App normal gestartet => PhotoChooser öffnen
                    State[fromChooser] = true;
                    photoChooser.Show();
                }
            }
        }

        private BitmapImage GetImage(Picture picture)
        {
            BitmapImage image = new BitmapImage();
            image.CreateOptions = BitmapCreateOptions.None;
            image.SetSource(picture.GetImage());
            return image;
        }
    }
}