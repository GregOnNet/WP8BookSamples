using System;
using System.Net;
using System.Windows.Navigation;

namespace UriAssociations.Core
{
  public class ProductUriMapper : UriMapperBase
  {
    public override Uri MapUri(Uri uri)
    {
      var uriString = HttpUtility.UrlDecode(uri.ToString());

      if (uriString.Contains("products:Product?Id="))
      {
        int productIdIndex = uriString.IndexOf("Id=") + 3;
        string productId = uriString.Substring(productIdIndex);

        return new Uri("/DetailsPage.xaml?Id=" + productId,
                       UriKind.Relative);
      }
      return uri;
    }
  }
}