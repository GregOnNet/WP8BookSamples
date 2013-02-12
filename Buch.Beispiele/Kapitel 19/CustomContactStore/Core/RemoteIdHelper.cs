using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Phone.PersonalInformation;

namespace CustomContactStore.Core
{
  public class RemoteIdHelper
  {
    private const string ContactStoreKey = "LocalAppContactId";

    public async Task TrySetRemoteIdGuid(ContactStore store)
    {
      var extendedProperties = await store.LoadExtendedPropertiesAsync().AsTask();
      if (!extendedProperties.ContainsKey(ContactStoreKey))
      {
        // the given store does not have a local instance id so set one against store extended properties
        var guid = Guid.NewGuid();
        extendedProperties.Add(ContactStoreKey, guid.ToString());
        
        var readonlyProperties = new ReadOnlyDictionary<string, object>(extendedProperties);
        await store.SaveExtendedPropertiesAsync(readonlyProperties);
      }
    }

    public async Task<string> GetTaggedRemoteId(ContactStore store, string remoteId)
    {
      var taggedRemoteId = String.Empty;
      var extendedProperties = await store.LoadExtendedPropertiesAsync();

      if (extendedProperties.ContainsKey(ContactStoreKey))
      {
        taggedRemoteId = string.Format("{0}_{1}", extendedProperties[ContactStoreKey], remoteId);
      }

      return taggedRemoteId;
    }

    public async Task<string> GetUntaggedRemoteId(ContactStore store, string taggedRemoteId)
    {
      var remoteId = String.Empty;

      var properties = await store.LoadExtendedPropertiesAsync();
      if (properties.ContainsKey(ContactStoreKey))
      {
        var localInstanceId = properties[ContactStoreKey] as String;
        
        if (!String.IsNullOrEmpty(localInstanceId) &&
            taggedRemoteId.Length > localInstanceId.Length + 1)
        {
          remoteId = taggedRemoteId.Substring(localInstanceId.Length + 1);
        }
      }

      return remoteId;
    }
  }
}