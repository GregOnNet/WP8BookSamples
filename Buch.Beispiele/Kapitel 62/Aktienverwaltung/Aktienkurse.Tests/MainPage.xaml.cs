using Microsoft.Phone.Controls;
using Microsoft.Phone.Testing;

namespace Aktienkurse.Tests
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            Content = UnitTestSystem.CreateTestPage();
        }
    }
}