using System.Windows.Media.Imaging;

namespace InAppPurchase.Filters
{
    public interface IFilter
    {
        string Name { get; set; }
        bool HasParameter { get; set; }
        double Minimum { get; }
        double Maximum { get; }
        double Value { get; set; }
        void Apply(WriteableBitmap image);
    }
}
