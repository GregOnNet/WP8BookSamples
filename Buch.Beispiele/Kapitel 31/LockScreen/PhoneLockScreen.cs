using System;
using System.Linq;
using Microsoft.Phone.Shell;
using Windows.Phone.System.UserProfile;

namespace PhoneLockScreen
{
  public class PhoneLockScreen
  {
    public static void SetInformation(int tileCount, string message)
    {
      var primaryTileToken = ShellTile.ActiveTiles.FirstOrDefault();

      var flipTileData = new FlipTileData
      {
        Count = tileCount,
        WideBackContent = message
      };

      if (primaryTileToken != null)
      {
        primaryTileToken.Update(flipTileData);
      }
    }

    public static void ClearInformation()
    {
      SetInformation(0, String.Empty);
    }

    public async static void SetBackgroundImage(Uri imageUri)
    {
      if (!LockScreenManager.IsProvidedByCurrentApplication)
      {
        var permission = await LockScreenManager.RequestAccessAsync();

        if (permission == LockScreenRequestResult.Denied)
        {
          return;
        }
      }

      LockScreen.SetImageUri(new Uri("ms-appx:///Assets/lockscreen.png", UriKind.Absolute));
    }
  }
}