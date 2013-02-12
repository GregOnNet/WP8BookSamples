using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InAppPurchase.Filters
{
    public class Sepia : IFilter
    {
        public Sepia()
        {
            Name = "Sepia";
            HasParameter = true;
            Minimum = 0;
            Maximum = 100;
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

            for (int i = 0; i < image.Pixels.Length; i++)
            {
                Color pixel = FilterHelper.GetColor(image.Pixels[i]);

                double greyValue = pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114;

                double r = greyValue + 2 * Value;
                if (r > 255)
                    r = 255;

                double g = greyValue + Value;
                if (g > 255)
                    g = 255;

                double b = greyValue;

                pixel.R = (byte)(r);
                pixel.G = (byte)(g);
                pixel.B = (byte)(b);

                image.Pixels[i] = FilterHelper.GetInt32(pixel);
            }
        }
    }
}
