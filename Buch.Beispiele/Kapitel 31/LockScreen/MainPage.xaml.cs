using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace PhoneLockScreen
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();
    }

    private void OnSettingLockScreen(object sender, GestureEventArgs e)
    {
      PhoneLockScreen.SetInformation(5, "Das nächste Work-Out ruft.");
    }

    private void OnClearingLockScreen(object sender, GestureEventArgs e)
    {
      PhoneLockScreen.ClearInformation();
    }
  }
}