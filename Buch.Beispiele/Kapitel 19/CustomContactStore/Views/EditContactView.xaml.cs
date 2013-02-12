using System;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace CustomContactStore.Views
{
  public partial class EditContactView : PhoneApplicationPage
  {
    public EditContactView()
    {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      if (NavigationContext.QueryString.ContainsKey("id") &&
          !String.IsNullOrEmpty(NavigationContext.QueryString["id"]))
      {
        //Nachriht, die das Laden von AktienDetails im ViewModel auslöst
        Messenger.Default.Send<string>(NavigationContext.QueryString["id"], "LoadContactByEmailMessage");
      }
    }
  }
}