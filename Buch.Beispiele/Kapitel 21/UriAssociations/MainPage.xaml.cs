using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Windows.System;

namespace UriAssociations
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();
    }

    private void OnUsingMappedUri(object sender, GestureEventArgs e)
    {
      Launcher.LaunchUriAsync(
        new Uri("products:Product?Id=f89bd"));
    }
  }
}