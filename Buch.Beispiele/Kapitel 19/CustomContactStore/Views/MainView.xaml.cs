using System.Linq;
using System.Windows.Input;
using CustomContactStore.Core;
using Microsoft.Phone.Controls;

namespace CustomContactStore.Views
{
  public partial class MainView : PhoneApplicationPage
  {
    private AppContacts _appContacts;

    public MainView()
    {
      InitializeComponent();
      this.Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      _appContacts = new AppContacts();
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      // Auswahl der Liste aufheben
      ContactsList.SelectedIndex = -1;
    }
  }
}