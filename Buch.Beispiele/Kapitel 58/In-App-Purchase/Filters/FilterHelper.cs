using System.Windows.Media;

namespace InAppPurchase.Filters
{
    internal static class FilterHelper
    {
        public static Color GetColor(int argb)
        {
            var a = (byte)(argb >> 24);
            var r = (byte)(argb >> 16);
            var g = (byte)(argb >> 8);
            var b = (byte)(argb);

            return Color.FromArgb(a, r, g, b);
        }

        public static int GetInt32(Color color)
        {
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }
    }
}
