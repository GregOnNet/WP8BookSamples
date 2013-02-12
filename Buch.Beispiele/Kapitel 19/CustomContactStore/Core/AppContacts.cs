using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CustomContactStore.ViewModels;
using Windows.Phone.PersonalInformation;

namespace CustomContactStore.Core
{
  public class AppContacts
  {
    private readonly RemoteIdHelper _remoteIdHelper;

    public AppContacts()
    {
      _remoteIdHelper = new RemoteIdHelper();
    }

    public ContactStore ContactStore { get; private set; }

    public async void Save(StoredContactViewModel contactViewModel)
    {
      await CheckForContactStore();

      var contact = new StoredContact(ContactStore)
                      {
                        RemoteId = await _remoteIdHelper.GetTaggedRemoteId(ContactStore, contactViewModel.EMail),
                        DisplayName = contactViewModel.NickName,
                        GivenName = contactViewModel.FirstName,
                        FamilyName = contactViewModel.FamilyName
                      };

      await contact.SaveAsync();
    }

    public async void Update(StoredContactViewModel contactViewModel)
    {
      await CheckForContactStore();

      var contact = await GetContactByEmail(contactViewModel.EMail);

      if (contact == null)
        return;

      contact.GivenName = contactViewModel.FirstName;
      contact.FamilyName = contactViewModel.FamilyName;
      contact.DisplayName = contactViewModel.NickName;

      contact.SaveAsync();
    }

    public async Task Remove(string email)
    {
      await CheckForContactStore();

      var contact = await GetContactByEmail(email);

      if (contact != null)
      {
        await ContactStore.DeleteContactAsync(contact.Id);
      }
    }

    public async Task<ObservableCollection<StoredContactViewModel>> GetAll()
    {
      await CheckForContactStore();

      var query = ContactStore.CreateContactQuery();
      var contacts = await query.GetContactsAsync();
      var contactCollection = new ObservableCollection<StoredContactViewModel>();

      foreach (var contact in contacts)
      {
        contactCollection.Add(
          new StoredContactViewModel
            {
              EMail = await _remoteIdHelper.GetUntaggedRemoteId(ContactStore, contact.RemoteId),
              FirstName = contact.GivenName,
              FamilyName = contact.FamilyName,
              NickName = contact.DisplayName
            });
      }

      return contactCollection;
    }

    public async Task<StoredContact> GetContactByEmail(string email)
    {
      await CheckForContactStore();

      var remoteId = await _remoteIdHelper.GetTaggedRemoteId(ContactStore, email);
      var contact = await ContactStore.FindContactByRemoteIdAsync(remoteId);

      return contact;
    }

    public async Task CheckForContactStore()
    {
      if (ContactStore != null)
        return;

      ContactStore = await ContactStore.CreateOrOpenAsync(
                              ContactStoreSystemAccessMode.ReadWrite, 
                              ContactStoreApplicationAccessMode.ReadOnly);

      // Unique Id für Contact-Save setzen
      await _remoteIdHelper.TrySetRemoteIdGuid(ContactStore);
    }
  }
}