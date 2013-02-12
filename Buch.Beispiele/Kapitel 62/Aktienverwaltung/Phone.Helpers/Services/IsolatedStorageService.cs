using System.IO.IsolatedStorage;

namespace Phone.Helpers.Services
{
    public static class IsolatedStorageService
    {
        public static void StoreObject<T>(T value, string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            }
        }

        public static void DeleteObject(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(key);
            }
        }

        public static T GetStoredObject<T>(string key)
        {
            T value;
            
            IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value);

            return value;
        }

        public static void Save()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}