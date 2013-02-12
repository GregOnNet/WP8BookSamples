using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InAppPurchase.Filters
{
    public class Brightness : IFilter
    {
        public Brightness()
        {
            Name = "Helligkeit";
            HasParameter = true;
            Minimum = -100;
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

            // Helligkeitsanpassung erfolgt durch addieren des gewünschten Betrages
            for (int i = 0; i < image.Pixels.Length; i++)
            {
                Color pixel = FilterHelper.GetColor(image.Pixels[i]);

                double r = pixel.R + Value;
                if (r > 255)
                    r = 255;
                if (r < 0)
                    r = 0;

                double g = pixel.G + Value;
                if (g > 255)
                    g = 255;
                if (g < 0)
                    g = 0;

                double b = pixel.B + Value;
                if (b > 255)
                    b = 255;
                if (b < 0)
                    b = 0;

                pixel.R = (byte)(r);
                pixel.G = (byte)(g);
                pixel.B = (byte)(b);

                image.Pixels[i] = FilterHelper.GetInt32(pixel);
            }
        }
    }
}
