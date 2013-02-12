using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;

namespace ShareLinkTaskApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
        }

        private void TapMessageTextBox(object sender, GestureEventArgs e)
        {
            MessageToShare.SelectAll(); 
        }

        private void TapShareIt(object sender, RoutedEventArgs e)
        {
            ShareLinkTask shareLinnkTask = new ShareLinkTask();

            shareLinnkTask.Title = "WP7 - sharing a link - Test";
            shareLinnkTask.LinkUri = new Uri(LinkToShare.Text, UriKind.Absolute);
            shareLinnkTask.Message = MessageToShare.Text;

            shareLinnkTask.Show();

        }
    }
}