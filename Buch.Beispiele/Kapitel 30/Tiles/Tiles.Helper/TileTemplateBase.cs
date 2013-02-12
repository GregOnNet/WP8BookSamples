using System;
using System.Linq;
using Microsoft.Phone.Shell;
using Tiles.Helper.Extensions;

namespace Tiles.Helper
{
  public class TileTemplateBase<T> where T : ShellTileData
  {
    public static ShellTile GetByUri(Uri targetUri)
    {
      var typedUri = targetUri.TypeForTileTemplate<T>();
      var tile = ShellTile.ActiveTiles.FirstOrDefault(u => u.NavigationUri.Equals(typedUri));

      return tile;
    }

    public static bool Exists(Uri targetUri)
    {
      return GetByUri(targetUri) != null;
    }

    public static void Remove(Uri targetUri)
    {
      var tile = GetByUri(targetUri);
      if (tile != null)
      {
        tile.Delete();
      }
    }
  }
}