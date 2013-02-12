using System;
using Microsoft.Phone.Shell;

namespace Tiles.Helper.Extensions
{
  public static class TileUriExtensions
  {
    public static Uri TypeForTileTemplate<T>(this Uri uri) where T : ShellTileData
    {
      string pathAndQuery = uri.OriginalString;

      pathAndQuery += pathAndQuery.EndsWith(".xaml")
                        ? TileParameters.ParameterInitializer
                        : TileParameters.ParameterConcatenator;

      if (typeof(FlipTileData) == typeof(T))
      {
        pathAndQuery += TileParameters.FlipTileParamter;
      }
      else if (typeof(IconicTileData) == typeof(T))
      {
        pathAndQuery += TileParameters.IconicTileParamter;
      }
      else if (typeof(CycleTileData) == typeof(T))
      {
        pathAndQuery += TileParameters.CycleTileParamter;
      }
      else
      {
        throw new ArgumentException(
          String.Format("The ShellDataType '{0}' is not supported.",
                        typeof (T).Name));
      }

      return new Uri(pathAndQuery, UriKind.RelativeOrAbsolute);
    }
  }
}