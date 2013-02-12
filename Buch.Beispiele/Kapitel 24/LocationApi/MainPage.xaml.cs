using System;
using System.Windows;
using System.Windows.Media;
using LocationApi.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Windows.Devices.Geolocation;

namespace LocationApi
{
  public partial class MainPage : PhoneApplicationPage
  {
    private Geolocator _geo;
    private MapPolyline _polyline;

    public MainPage()
    {
      InitializeComponent();
      Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
      InitializePolyLineOnMap();

      _geo = new Geolocator
      {
        MovementThreshold = 25.0,
        DesiredAccuracyInMeters = 50
      };

      _geo.StatusChanged += OnStatusChanged;
      _geo.PositionChanged += OnPositionChanged;
    }

    private void OnStatusChanged(Geolocator geolocator, StatusChangedEventArgs args)
    {
      Dispatcher.BeginInvoke(
        () => Status.Text = args.Status.ToString());
    }

    private void OnPositionChanged(Geolocator geolocator, PositionChangedEventArgs args)
    {
      if(App.RunningInBackground)
      {
        var toast = new Microsoft.Phone.Shell.ShellToast
        {
          Content = args.Position.Coordinate.Latitude.ToString("0.00"),
          Title = "Standort: ",
          NavigationUri = new Uri("/MainPage.xaml", UriKind.Relative)
        };

        toast.Show();
      }
      else
      {
        Dispatcher.BeginInvoke(() =>
        {
          Latitude.Text = args.Position.Coordinate.Latitude.ToString("F5");
          Longitude.Text = args.Position.Coordinate.Longitude.ToString("F5");

          _polyline.Path.Add(args.Position.ToGeoCoodinate());

          Map.SetView(args.Position.ToGeoCoodinate(), 16);
        });
      }
    }

    private void InitializePolyLineOnMap()
    {
      _polyline = new MapPolyline
      {
        Path = new GeoCoordinateCollection(),
        StrokeColor = Colors.Red,
        StrokeThickness = 3
      };

      Map.MapElements.Add(_polyline);
    }
  }
}