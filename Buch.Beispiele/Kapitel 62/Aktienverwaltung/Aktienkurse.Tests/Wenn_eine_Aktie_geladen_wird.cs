using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aktienkurse.ViewModels;
using Aktienkurse.Tests.Fakes;
using System.Threading.Tasks;

namespace Aktienkurse.Tests
{
  [TestClass]
  public class Wenn_eine_Aktie_geladen_wird
  {
    [TestMethod]
    public async void soll_bei_fehlender_Internetverbindung_keine_Aktieninstanz_geladen_werden()
    {
      var aktieViewModel = new AktieViewModel();
      aktieViewModel.NetworkService = new FakeNetworkNotAvailableService();

      await Task.Run(() => aktieViewModel.GetAktieDetailsBySymbolAsync("msft"));

      Assert.IsFalse(aktieViewModel.HasValidValue);
    }

    [TestMethod]
    public void soll_diese_nicht_gespeichert_werden_wenn_sie_keine_gültigen_Daten_enthält()
    {
      var aktieViewModel = new AktieViewModel
                             {
                               HasValidValue = false
                             };

      Assert.IsFalse(aktieViewModel.CanSave());
    }

    [TestMethod]
    public void soll_diese_gespeichert_werden_wenn_sie_gültigen_Daten_enthält()
    {
      var aktieViewModel = new AktieViewModel
                             {
                               HasValidValue = true
                             };

      Assert.IsTrue(aktieViewModel.CanSave());
    }
  }
}
