using System.Collections.Generic;
using System.Threading.Tasks;
using Aktienkurse.DataService;
using Phone.Helpers.Services;
using Aktienkurse.DataService.Model;

namespace Aktienkurse.Services
{
  public class AktienStorageService
  {
    private const string AccessKey = "Symbole";
    private readonly NetworkService _networkService;
    /// <summary>
    /// Initialisierung der Klasse
    /// - Initialisierung des Networkservices,
    ///   um Konnektivität zum WAN zu testen.
    /// </summary>
    public AktienStorageService()
    {
      _networkService = new NetworkService();
    }

    /// <summary>
    /// Fügt ein Aktiensymbol in die im 
    /// IsolatedStorage gespeicherte Symbolliste hinzu.
    /// </summary>
    /// <param name="aktienGesellschaftsSymbol">Aktiensymbol eines Unternehmens</param>
    public void AddAktie(string aktienGesellschaftsSymbol)
    {
      var symbole = IsolatedStorageService.GetStoredObject<List<string>>(AccessKey);

      if (symbole == null)
      {
        symbole = new List<string>();
      }

      symbole.Add(aktienGesellschaftsSymbol);

      IsolatedStorageService.StoreObject(symbole, AccessKey);
      IsolatedStorageService.Save();
    }

    /// <summary>
    /// Lädt die im IsolatedStorage gespeicherte Symbolliste
    /// </summary>
    /// <returns>Symbolliste aller gespeicherten Aktiengesellschaften</returns>
    public List<string> LoadAktienSymbole()
    {
      return IsolatedStorageService
             .GetStoredObject<List<string>>(AccessKey);
    }

    /// <summary>
    /// Lädt Aktiengesellschaft zum angegebenen Aktiensymbol
    /// </summary>
    /// <param name="aktienSymbol">Aktiensymbol eines Unternehmens</param>
    /// <returns>Aktiengesellschaft mit aktuellen Kursinformationen</returns>
    public async Task<Aktie> GetAktietBySymbol(string aktienSymbol)
    {
      if (string.IsNullOrEmpty(aktienSymbol) ||
          _networkService.IsAvailable == false)
      {
        return null;
      }

      var manager = new AktienManager();

      return await manager.GetAktieBySymbol(aktienSymbol);
    }

    public void DeleteAktie(string symbol)
    {
      var symbolList = IsolatedStorageService.GetStoredObject<List<string>>(AccessKey);

      if (symbolList != null)
      {
        symbolList.Remove(symbol);
        IsolatedStorageService.Save();
      }
    }
  }
}