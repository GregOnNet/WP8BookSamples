using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Globalization;

namespace Lokalisierung
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Konstruktor
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var germanCulture = new CultureInfo("de-DE");
			var englishCulture = new CultureInfo("en-US");
			
			decimal money = 1284.95m;
			GermanDate.Text = money.ToString("C", germanCulture);
			EnglishDate.Text = money.ToString("C", englishCulture);

			DataContext = money;

			MessageBox.Show(AppResources.Message);
		}
	}
}