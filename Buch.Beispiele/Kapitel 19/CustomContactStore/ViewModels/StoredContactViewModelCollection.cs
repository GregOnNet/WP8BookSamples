using System;
using System.Collections.ObjectModel;
using CustomContactStore.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Phone.Helpers.Services;

namespace CustomContactStore.ViewModels
{
  public class StoredContactViewModelCollection : ViewModelBase
  {
    private readonly AppContacts _appContacts;
    const string EditPage = "/Views/EditContactView.xaml";

    public StoredContactViewModelCollection()
    {
      if (IsInDesignMode) return;

      _appContacts = new AppContacts();
      LoadContacts();

      LoadContactsCommand = new RelayCommand(LoadContacts);
      NavigateToEditContactPageCommand = new RelayCommand<StoredContactViewModel>(NavigateToEditContactPage);
    }

    private ObservableCollection<StoredContactViewModel> _storedContacts;
    public ObservableCollection<StoredContactViewModel> StoredContacts
    {
      get
      {
        return _storedContacts ??
              (_storedContacts = _storedContacts = new ObservableCollection<StoredContactViewModel>());
      }
      set
      {
        _storedContacts = value;
        RaisePropertyChanged("StoredContacts");
      }
    }

    //Commands
    public RelayCommand LoadContactsCommand { get; set; }
    public RelayCommand<StoredContactViewModel> NavigateToEditContactPageCommand { get; set; }

    private async void LoadContacts()
    {
      StoredContacts.Clear();
      StoredContacts = await _appContacts.GetAll();
    }

    private void NavigateToEditContactPage(StoredContactViewModel contact)
    {
      if (contact != null)
      {
        AppNavigationService.NavigateTo(
          new Uri(EditPage + "?id=" + contact.EMail, UriKind.Relative));
      }
    }
  }
}