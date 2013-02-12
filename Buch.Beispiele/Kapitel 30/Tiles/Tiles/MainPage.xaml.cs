using System;
using System.Windows;
using System.Windows.Input;
using Tiles.Helper;
using Tiles.Helper.Extensions;

namespace Tiles
{
  public partial class MainPage
  {
    private static readonly Uri MainPageUri = new Uri("/MainPage.xaml", UriKind.Relative);

    public MainPage()
    {
      InitializeComponent();
      this.Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
      ToggleFlipTile.IsChecked = FlipTemplate.Exists(MainPageUri);
      ToggleIconicTile.IsChecked = IconicTemplate.Exists(MainPageUri);
      ToggleCycleTile.IsChecked = CycleTemplate.Exists(MainPageUri);
    }

    private void OnTogglingFlipTile(object sender, GestureEventArgs e)
    {
      if (ToggleFlipTile.IsChecked.GetValueOrDefault())
      {
        FlipTemplate.CreateOrUpdate(FlipTileNewsCount.Text.GetIntOrZero(),
                                    MainPageUri,
                                    ToggleWideFlipTile.IsChecked.GetValueOrDefault());
      }
      else
      {
        FlipTemplate.Remove(MainPageUri);
      }
    }

    private void OnTogglingIconicTile(object sender, GestureEventArgs e)
    {
      if (ToggleIconicTile.IsChecked.GetValueOrDefault())
      {
        IconicTemplate.CreateOrUpdate(IconicTileNewsCount.Text.GetIntOrZero(),
                                      MainPageUri,
                                      true,
                                      "Das sollten Sie wissen",
                                      "Für die ersten 4 Züge beim Schach gibt es",
                                      "318979564000 verschiedene Möglichkeiten.");
      }
      else
      {
        IconicTemplate.Remove(MainPageUri);
      }
    }

    private void OnTogglingCycleTile(object sender, GestureEventArgs e)
    {
      if (ToggleCycleTile.IsChecked.GetValueOrDefault())
      {
        CycleTemplate.CreateOrUpdate(
          CycleTileNewsCount.Text.GetIntOrZero(),
          MainPageUri,
          CycleWideFlipTile.IsChecked.GetValueOrDefault());
      }
      else
      {
        CycleTemplate.Remove(MainPageUri);
      }
    }

    private void OnAllowWideFlipTileChanged(object sender, GestureEventArgs gestureEventArgs)
    {
      if (FlipTemplate.Exists(MainPageUri))
      {
        FlipTemplate.Remove(MainPageUri);
        ToggleFlipTile.IsChecked = false;
      }
    }

    private void OnAllowWideIconicTileChanged(object sender, GestureEventArgs e)
    {
      if (IconicTemplate.Exists(MainPageUri))
      {
        IconicTemplate.Remove(MainPageUri);
        ToggleIconicTile.IsChecked = false;
      }
    }

    private void OnAllowWideCycleTileChanged(object sender, GestureEventArgs e)
    {
      if (CycleTemplate.Exists(MainPageUri))
      {
        CycleTemplate.Remove(MainPageUri);
        ToggleCycleTile.IsChecked = false;
      }
    }
  }
}