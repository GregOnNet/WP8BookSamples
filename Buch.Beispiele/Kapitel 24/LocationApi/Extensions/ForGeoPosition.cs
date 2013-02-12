using System.Device.Location;
using Windows.Devices.Geolocation;

namespace LocationApi.Extensions
{
  public static class ForGeoposition
  {
     public static GeoCoordinate ToGeoCoodinate(this Geoposition p)
     {
       return new GeoCoordinate(p.Coordinate.Latitude,
                                p.Coordinate.Longitude);
     }
  }
}