using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DataBinding.Converter
{
    public class ImageToUriConverter : IValueConverter
    {

        public object Convert(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var image = value as BitmapImage;
            if(image == null)
            {
                return "kein Uri zum Bild gefunden!";
            }
            return image.UriSource.ToString();
        }

        public object ConvertBack(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string uri = value as string;

            if(string.IsNullOrEmpty(uri))
            {
                return null;
            }

            Uri sourceUri = new Uri(uri, UriKind.Relative);

            BitmapImage image = new BitmapImage
            {
                UriSource = sourceUri
            };

            return image;
        }
    }
}