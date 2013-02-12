namespace Tiles.Helper
{
  public class TileParameters
  {
    public static string ParameterInitializer { get { return "?"; } }
    public static string ParameterConcatenator { get { return "&"; } }

    public static string FlipTileParamter { get { return "shellTemplateType=FlipTemplate"; } }
    public static string IconicTileParamter { get { return "shellTemplateType=IconicTemplate"; } }
    public static string CycleTileParamter { get { return "shellTemplateType=CycleTemplate"; } } 
  }
}