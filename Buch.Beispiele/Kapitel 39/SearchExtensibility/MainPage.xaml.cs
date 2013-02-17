using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SearchExtensibility.Resources;

namespace SearchExtensibility
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("ProductName"))
            {
                string productName = NavigationContext.QueryString["ProductName"];
                SearchBox.Text = productName;
                Search(productName);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Search(SearchBox.Text);
        }

        private void Search(string query)
        {
            string searchTerm = query.Replace(" ", "+");
            string searchUrl = "http://www.heise.de/preisvergleich/?fs=" + searchTerm;
            WebBrowser1.Navigate(new Uri(searchUrl, UriKind.Absolute));
        }
    }
}