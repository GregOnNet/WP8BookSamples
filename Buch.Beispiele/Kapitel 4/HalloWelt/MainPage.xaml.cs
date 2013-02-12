using System.Windows;
using Microsoft.Phone.Controls;

namespace HalloWelt
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hallo " + textBox1.Text);
        }
    }
}