using System;
using System.IO;

namespace FileAssociations.Extensions
{
  public static class ForUriWithFileInformation
  {
    public static string GetFileToken(this String uriString)
    {
      int fileIdIndex = uriString.IndexOf("fileToken=") + 10;
      return uriString.Substring(fileIdIndex);
    }

    public static string Extension(this String fileName)
    {
      return Path.GetExtension(fileName);
    }
  }
}