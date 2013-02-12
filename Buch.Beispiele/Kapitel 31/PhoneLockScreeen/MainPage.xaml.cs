using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace PhoneLockScreeen
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
      PhoneLock.SetInformation(5, "Das nächste Work-Out ruft.");
    }

    private void OnClearingLockScreen(object sender, GestureEventArgs e)
    {
      PhoneLock.ClearInformation();
    }

    private void OnSettingBackgroundImage(object sender, GestureEventArgs e)
    {
      PhoneLock.SetBackgroundImage(new Uri("ms-appx:///Assets/LockScreen/LockScreenBackground.jpg", UriKind.Absolute));
    }
  }
}