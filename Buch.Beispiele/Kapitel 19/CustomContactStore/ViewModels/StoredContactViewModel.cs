using CustomContactStore.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Phone.Helpers.Services;

namespace CustomContactStore.ViewModels
{
  public class StoredContactViewModel : ViewModelBase
  {
    private readonly AppContacts _appContacts;

    public StoredContactViewModel()
    {
      _appContacts = new AppContacts();

      SaveCommand = new RelayCommand(Save);
      UpdateCommand = new RelayCommand(Update);
      RemoveCommand = new RelayCommand(Remove);

      Messenger.Default.Register<string>(this, "LoadContactByEmailMessage", LoadContactByEmail);
    }


    private string _firstName;
    public string FirstName
    {
      get { return _firstName; }
      set
      {
        _firstName = value;
        RaisePropertyChanged("FirstName");
      }
    }

    private string _familyName;
    public string FamilyName
    {
      get { return _familyName; }
      set
      {
        _familyName = value;
        RaisePropertyChanged("FamilyName");
      }
    }

    private string _nickName;
    public string NickName
    {
      get { return _nickName; }
      set
      {
        _nickName = value;
        RaisePropertyChanged("NickName");
      }
    }

    private string _email;
    public string EMail
    {
      get { return _email; }
      set
      {
        _email = value;
        RaisePropertyChanged("EMail");
      }
    }

    // Commands
    public RelayCommand SaveCommand { get; set; }
    public RelayCommand UpdateCommand { get; set; }
    public RelayCommand RemoveCommand { get; set; }

    public void Save()
    {
      _appContacts.Save(this);
    }

    public void Update()
    {
      _appContacts.Update(this);
      AppNavigationService.GoBack();
    }

    public void Remove()
    {
      _appContacts.Remove(EMail);
      AppNavigationService.GoBack();
    }

    private async void LoadContactByEmail(string email)
    {
      var contact = await _appContacts.GetContactByEmail(email);

      EMail = email;
      FirstName = contact.GivenName;
      FamilyName = contact.FamilyName;
      NickName = contact.DisplayName;
    }
  }
}