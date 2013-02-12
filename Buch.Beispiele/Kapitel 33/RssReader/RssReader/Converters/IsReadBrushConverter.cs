using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RssReader.Converters
{
	public class IsReadBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool isRead = (bool)value;
			if (isRead)
			{
				return (Brush)Application.Current.Resources["PhoneForegroundBrush"];
			}
			else
			{
				return (Brush)Application.Current.Resources["PhoneAccentBrush"];
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
