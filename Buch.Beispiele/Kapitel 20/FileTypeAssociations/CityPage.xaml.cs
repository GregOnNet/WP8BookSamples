using System;
using System.IO;
using System.Threading.Tasks;
using FileAssociations.Models;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Windows.Phone.Storage.SharedAccess;
using Windows.Storage;

namespace FileAssociations
{
  public partial class CityPage : PhoneApplicationPage
  {
    private StorageFolder _filesFolder;
    private IStorageFile _storedFile;

    public CityPage()
    {
      InitializeComponent();
    }

    protected override async void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      if (NavigationContext.QueryString.ContainsKey("fileToken"))
      {
        await CreateOrOpenCityFolderAsync();
        await SaveFileToStorageAsync();

        var city = await GetStoredCityAsync();

        CityName.Text = city.Name;
        State.Text = city.State;
        Country.Text = city.Country;
        Population.Text = city.Population.ToString();
      }
    }

    private async Task<City> GetStoredCityAsync()
    {
      var fileStream = await _storedFile.OpenReadAsync();
      var streamReader = new StreamReader(fileStream.AsStream());

      string json = streamReader.ReadToEnd();
      var city = JsonConvert.DeserializeObject<City>(json);

      return city;
    }

    private async Task SaveFileToStorageAsync()
    {
      string fileToken = NavigationContext.QueryString["fileToken"];
      string fileName = SharedStorageAccessManager.GetSharedFileName(fileToken);

      _storedFile = await SharedStorageAccessManager
                      .CopySharedFileAsync(_filesFolder,
                                           fileName,
                                           NameCollisionOption.ReplaceExisting,
                                           fileToken);
    }

    private async Task CreateOrOpenCityFolderAsync()
    {
      _filesFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(
        "Cities", CreationCollisionOption.OpenIfExists);
    }
  }
}