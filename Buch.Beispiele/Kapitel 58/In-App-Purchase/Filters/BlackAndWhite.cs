using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InAppPurchase.Filters
{
    public class BlackAndWhite : IFilter
    {
        public BlackAndWhite()
        {
            Name = "S/W";
            HasParameter = false;
        }

        public string Name { get; set; }
        public bool HasParameter { get; set; }
        public double Minimum { get; private set; }
        public double Maximum { get; private set; }
        public double Value { get; set; }

        public void Apply(WriteableBitmap image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            // Jeder Pixel erhält den selben Wert in allen Farbkanälen
            // die ungleichmäßige Berücksichtigung der Kanäle bewirkt eine natürliche Farbwiedergabe
            for (int i = 0; i < image.Pixels.Length; i++)
            {
                Color pixel = FilterHelper.GetColor(image.Pixels[i]);
                pixel.R = (byte)(pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114);
                pixel.G = pixel.R;
                pixel.B = pixel.R;

                image.Pixels[i] = FilterHelper.GetInt32(pixel);
            }
        }
    }
}
