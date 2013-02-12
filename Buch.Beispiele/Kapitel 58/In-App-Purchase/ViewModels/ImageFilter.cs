using InAppPurchase.Filters;
using System.Windows;
using System.Windows.Media;

namespace InAppPurchase.ViewModels
{
    public class ImageFilter : ViewModelBase
    {
        public ImageFilter(IFilter filter)
        {
            Filter = filter;
            Name = filter.Name;
            HasParameter = filter.HasParameter ? Visibility.Visible : Visibility.Collapsed;
            Mininum = filter.Minimum;
            Maximum = filter.Maximum;
        }

        public IFilter Filter { get; private set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                SendPropertyChanged();
            }
        }

        private ImageSource _image;

        public ImageSource Image
        {
            get { return _image; }
            set
            {
                if (_image == value)
                    return;

                _image = value;
                SendPropertyChanged();
            }
        }

        private Visibility _hasParameter;

        public Visibility HasParameter
        {
            get { return _hasParameter; }
            set
            {
                if (_hasParameter == value)
                    return;

                _hasParameter = value;
                SendPropertyChanged();
            }
        }

        private double _mininum;

        public double Mininum
        {
            get { return _mininum; }
            set
            {
                if (_mininum == value)
                    return;

                _mininum = value;
                SendPropertyChanged();
            }
        }

        private double _maximum;

        public double Maximum
        {
            get { return _maximum; }
            set
            {
                if (_maximum == value)
                    return;

                _maximum = value;
                SendPropertyChanged();
            }
        }

        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;

                if (Filter != null)
                {
                    Filter.Value = value;
                }

                SendPropertyChanged();
            }
        }
    }
}
