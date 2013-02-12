using System;

namespace Tiles.Helper.Extensions
{
  public static class StringExtensions
  {
     public static int GetIntOrZero(this String text)
     {
       int number;
       if (int.TryParse(text, out number))
       {
         return number;
       }
       
       return 0;
     }
  }
}