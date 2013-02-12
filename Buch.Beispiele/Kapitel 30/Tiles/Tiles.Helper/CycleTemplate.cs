using System;
using System.Collections.Generic;
using Microsoft.Phone.Shell;
using Tiles.Helper.Extensions;

namespace Tiles.Helper
{
  public class CycleTemplate : TileTemplateBase<CycleTileData>
  {
    public static void CreateOrUpdate(int newsCount, Uri targetUri, bool supportsWideTile)
    {
      var tile = GetByUri(targetUri);

      var cycleTile = new CycleTileData
                        {
                          Title = "CYCLE TEMPLATE TILE",
                          Count = newsCount,
                          CycleImages = new List<Uri>
                                          {
                                            new Uri("Assets/Tiles/CycleTile/Tiles-1.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-2.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-3.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-4.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-5.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-6.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-7.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-8.jpg", UriKind.Relative),
                                            new Uri("Assets/Tiles/CycleTile/Tiles-9.jpg", UriKind.Relative),
                                          }
                        };

      tile.CreateOrUpdate<CycleTileData>(targetUri, cycleTile, supportsWideTile);
    }
  }
}