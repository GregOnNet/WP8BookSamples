using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Aktienkurse.Controls.Converters
{
    public class ValueToFontColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number;
            if (double.TryParse(value.ToString().Replace("%", ""), out number))
            {
                if (number > 0)
                {
                    return new SolidColorBrush(Colors.Green);
                }

                if (number < 0)
                {
                    return new SolidColorBrush(Colors.Red);
                }

            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}