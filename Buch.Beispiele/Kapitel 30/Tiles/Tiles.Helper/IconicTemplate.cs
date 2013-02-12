using System;
using Microsoft.Phone.Shell;
using Tiles.Helper.Extensions;

namespace Tiles.Helper
{
  public class IconicTemplate : TileTemplateBase<IconicTileData>
  {
    public static void CreateOrUpdate(int newsCount, 
                                      Uri targetUri, 
                                      bool supportsWideTile,
                                      string wideContent1,
                                      string wideContent2,
                                      string wideContent3)
    {
      var tile = GetByUri(targetUri);

      var iconicTile = new IconicTileData
                         {
                           Title = "ICONIC TEMPLATE TILE",
                           Count = newsCount,
                           WideContent1 = wideContent1,
                           WideContent2 = wideContent2,
                           WideContent3 = wideContent3
                         };

      tile.CreateOrUpdate<IconicTileData>(targetUri,
                                          iconicTile,
                                          supportsWideTile);
    }
  }
}