using System;
using System.IO;
using System.Windows.Navigation;
using FileAssociations.Extensions;
using Windows.Phone.Storage.SharedAccess;

namespace FileAssociations.Core
{
  public class CityUriMapper : UriMapperBase
  {
    public override Uri MapUri(Uri uri)
    {
      var uriString = uri.ToString();

      if (uriString.Contains("/FileTypeAssociation"))
      {
        string fileToken = uriString.GetFileToken();
        string fileName = SharedStorageAccessManager
                              .GetSharedFileName(fileToken);

        if (!String.IsNullOrEmpty(fileName) &&
            fileName.Extension().Equals(".city"))
        {
          return new Uri("/CityPage.xaml?fileToken=" + fileToken, UriKind.Relative);
        }
      }

      return uri;
    }
  }
}