using System;
using Microsoft.Phone.Shell;

namespace Tiles.Helper.Extensions
{
  public static class ShellTileExtensions
  {
    public static void CreateOrUpdate<T>(this ShellTile tile, 
                                              Uri targetUri, 
                                              T tileData, 
                                              bool supportsWideTile) where T : ShellTileData
    {
      if (tile == null)
      {
        ShellTile.Create(targetUri.TypeForTileTemplate<T>(), tileData, supportsWideTile);
      }
      else
      {
        tile.Update(tileData);
      }
    }
  }
}