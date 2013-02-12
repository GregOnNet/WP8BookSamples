using Microsoft.Phone.Controls;

namespace UriAssociations
{
  public partial class DetailsPage : PhoneApplicationPage
  {
    public DetailsPage()
    {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      if (NavigationContext.QueryString.ContainsKey("Id"))
      {
        PageName.Text = NavigationContext.QueryString["Id"];
      }
    }
  }
}