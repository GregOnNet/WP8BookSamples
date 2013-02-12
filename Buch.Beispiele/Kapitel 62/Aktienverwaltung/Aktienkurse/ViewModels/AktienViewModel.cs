using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Aktienkurse.DataService;
using Aktienkurse.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Phone.Helpers.Services;
using Aktienkurse.DataService.Model;

namespace Aktienkurse.ViewModels
{
    /// <summary>
    /// stellt Datenmodell für  View berei
    /// </summary>
    public class AktienViewModel : BasisViewModel
    {
      /// <summary>
        /// Dienst zum Laden von Aktieninformationen
        /// </summary>
        private readonly AktienStorageService _aktienStorage;

        /// <summary>
        /// Liste von AktieViewModels, die in der View gebunden wird
        /// </summary>
        public ObservableCollection<AktieViewModel> Aktien { get; private set; }
        
        /// <summary>
        /// enthält im Isolated Storage gespeicherte Symbole
        /// </summary>
        private List<string> _symbole;

        /// <summary>
        /// navigiert zur AktieDetailsView
        /// </summary>
        public RelayCommand<AktieViewModel> NavigateToDetailsCommand { get; set; }

        /// <summary>
        /// Konstruktor
        /// Initialisierung der Commands
        /// Registrierung von Mesages
        /// </summary>
        public AktienViewModel()
        {
            _aktienStorage = new AktienStorageService();

            Aktien = new ObservableCollection<AktieViewModel>();
            NavigateToDetailsCommand = new RelayCommand<AktieViewModel>(NavigateToDetails);

            //empfängt Nachricht aus View, sobald die Symbolliste neu geladen werden soll
            Messenger.Default.Register<object>(this, "UpdateListMessage", (o) => UpdateAsync());
        }
        
        /// <summary>
        /// Lädt Daten asynchron aus dem Model
        /// </summary>
        public async void UpdateAsync()
        {
            IsBusy = true;

            _symbole = LoadSymboleFromLocalStorage();

            if (_symbole != null && _symbole.Count > 0)
            {
                var manager = new AktienManager();
                var aktuelleAktien = await manager.GetAktienBySymbole(_symbole.ToArray());
                Aktien.Clear();
                foreach (var item in aktuelleAktien)
                {
                    var vm = new AktieViewModel();
                    vm.LoadValues(item);
                    Aktien.Add(vm);
                }
            }

            IsBusy = false;
        }
        
        /// <summary>
        /// Lädt gespeicherte Symbole aus dem Isolated Storage
        /// </summary>
        /// <returns>Symbole, die im Isolated Storage gespeichert sind</returns>
        private List<string> LoadSymboleFromLocalStorage()
        {
            return _aktienStorage.LoadAktienSymbole();
        }

        /// <summary>
        /// Durch NavigateToDetailsCommand gerufene 'Execute'-Methode, die zur Details-
        /// seite einer Aktie navigiert
        /// </summary>
        /// <param name="aktieViewModel">Aktie zuderen Details navigiert werden soll.</param>
        public void NavigateToDetails(AktieViewModel aktieViewModel)
        {
            if (aktieViewModel == null)
                return;

            var uri = new Uri("/Views/AktieDetailsView.xaml?symbol=" +
                              aktieViewModel.Symbol, UriKind.Relative);

            AppNavigationService.NavigateTo(uri);
        }
    }
}