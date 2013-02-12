using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InAppPurchase.Filters
{
    public class Contrast : IFilter
    {
        public Contrast()
        {
            Name = "Kontrast";
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
            
            // Kontrast = ((100 + X) / 100)²
            double contrast = (100.0 + Value) / 100.0;
            contrast *= contrast;

            // Kontrastanpassung erfolgt auf dem Bereich [-1,1] und wird durch Multiplikation mit dem Faktor erreicht
            for (int i = 0; i < image.Pixels.Length; i++)
            {
                Color pixel = FilterHelper.GetColor(image.Pixels[i]);

                double r = pixel.R / 255.0;
                r -= 0.5;
                r *= contrast;
                r += 0.5;
                r *= 255;
                if (r > 255)
                    r = 255;
                if (r < 0)
                    r = 0;

                double g = pixel.G / 255.0;
                g -= 0.5;
                g *= contrast;
                g += 0.5;
                g *= 255;
                if (g > 255)
                    g = 255;
                if (g < 0)
                    g = 0;

                double b = pixel.B / 255.0;
                b -= 0.5;
                b *= contrast;
                b += 0.5;
                b *= 255;
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
