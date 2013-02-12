using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Tiles.SimpleApiUsage
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();

    }

    private void OnCreatingFlipTile(object sender, GestureEventArgs e)
    {
      var tile = ShellTile.ActiveTiles.FirstOrDefault();

      if (tile != null)
      {
        var fliptile = new FlipTileData
        {
          Title = "Vorderseite",
          Count = 9,
          BackTitle = "Rückseite",
          BackContent = "Inhalt Rückseite",
          WideBackContent = "Rückseite des Wide-Tiles",
          SmallBackgroundImage = new Uri("Assets/FlipTile/159.jpg",
                                         UriKind.Relative),
          BackgroundImage = new Uri("Assets/FlipTile/336.jpg",
                                    UriKind.Relative),
          WideBackgroundImage = new Uri("Assets/FlipTile/wide.jpg",
                                        UriKind.Relative)
        };

        tile.Update(fliptile);
      }
    }

    private void OnCreatingIconicTile(object sender, GestureEventArgs e)
    {
      var icontile = new IconicTileData
      {
        Title = "Sport",
        Count = 12,
        IconImage = new Uri("Assets/IconicTile/202.png", UriKind.Relative),
        SmallIconImage = new Uri("Assets/IconicTile/38.png", UriKind.Relative),
        WideContent1 = "Wussten Sie schon...",
        WideContent2 = "1975 erziehlte Coby Orr ein Hole-In-One.",
        WideContent3 = "Er war fünf Jahre alt."
      };

      ShellTile.Create(new Uri("/MainPage.xaml", UriKind.Relative), icontile, true);
    }

    private void OnCreatingCycleTile(object sender, GestureEventArgs e)
    {
      var cycleTile = new CycleTileData
      {
        Title = "Leipzig Impressionen",
        Count = 3,
        SmallBackgroundImage = new Uri("Assets/CycleTile/Tiles-2.jpg", UriKind.Relative),
        CycleImages = new List<Uri>
        {
          new Uri("Assets/CycleTile/Tiles-1.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-2.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-3.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-4.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-5.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-6.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-7.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-8.jpg", UriKind.Relative),
          new Uri("Assets/CycleTile/Tiles-9.jpg", UriKind.Relative),
        }
      };

      ShellTile.Create(new Uri("/MainPage.xaml?shellTemplateType=CycleTemplate",
                      UriKind.Relative), 
                      cycleTile,
                      true);
    }
  }
}