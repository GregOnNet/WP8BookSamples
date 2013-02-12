using System;
using Microsoft.Phone.Shell;
using Tiles.Helper.Extensions;

namespace Tiles.Helper
{
  public class FlipTemplate : TileTemplateBase<FlipTileData>
  {
    public static void CreateOrUpdate(int newsCount, Uri targetUri, bool supportsWideTile)
    {
      var tile = GetByUri(targetUri);

      var flipTile = new FlipTileData
                       {
                         Title = "FLIP TEMPLATE TILE",
                         Count = newsCount
                       };

      tile.CreateOrUpdate<FlipTileData>(targetUri,
                                        flipTile,
                                        supportsWideTile);
    }
  }
}