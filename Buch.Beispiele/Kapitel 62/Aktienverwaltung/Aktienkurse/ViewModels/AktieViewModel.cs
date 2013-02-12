using Aktienkurse.Services;
using GalaSoft.MvvmLight.Command;
using Phone.Helpers.Services;
using GalaSoft.MvvmLight.Messaging;
using Aktienkurse.DataService.Model;

namespace Aktienkurse.ViewModels
{
    public class AktieViewModel : BasisViewModel
    {
        private AktienStorageService _aktienStorage;

      /// <summary>
      /// Konstruktor
      /// Initialisierung der Commands
      /// </summary>
      public AktieViewModel()
      {
        PropertyChanged += AktieViewModel_PropertyChanged;
        _aktienStorage = new AktienStorageService();

        SaveAktieCommand = new RelayCommand(Save, CanSave);
        GetAktieBySymbolCommand = new RelayCommand<string>(GetAktieDetailsBySymbolAsync);
        DeleteAktieCommand = new RelayCommand<string>(Delete);
      }

      //Eigenschaften, die von der View gebunden werden können
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    SendPropertyChanged("Name");
                }
            }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    SendPropertyChanged("Symbol");
                    HasValidValue = false;
                    UpdateCanAktieBeSaved(false);
                }
            }
        }

        private double _änderungNominal;
        public double ÄnderungNominal
        {
            get { return _änderungNominal; }
            set
            {
                if (_änderungNominal != value)
                {
                    _änderungNominal = value;
                    SendPropertyChanged("ÄnderungNominal");
                }
            }
        }

        private double _änderungProzentual;
        public double ÄnderungProzentual
        {
            get { return _änderungProzentual; }
            set
            {
                if (_änderungProzentual != value)
                {
                    _änderungProzentual = value;
                    SendPropertyChanged("ÄnderungProzentual");
                }
            }
        }

        private double _tagestief;
        public double Tagestief
        {
            get { return _tagestief; }
            set
            {
                if (_tagestief != value)
                {
                    _tagestief = value;
                    SendPropertyChanged("Tagestief");
                }
            }
        }

        private double _tageshoch;
        public double Tageshoch
        {
            get { return _tageshoch; }
            set
            {
                if (_tageshoch != value)
                {
                    _tageshoch = value;
                    SendPropertyChanged("Tageshoch");
                }
            }
        }

        private double _jahrestief;
        public double Jahrestief
        {
            get { return _jahrestief; }
            set
            {
                if (_jahrestief != value)
                {
                    _jahrestief = value;
                    SendPropertyChanged("Jahrestief");
                }
            }
        }

        private double _jahreshoch;
        public double Jahreshoch
        {
            get { return _jahreshoch; }
            set
            {
                if (_jahreshoch != value)
                {
                    _jahreshoch = value;
                    SendPropertyChanged("Jahreshoch");
                }
            }
        }

        private double _volumen;
        public double Volumen
        {
            get { return _volumen; }
            set
            {
                if (_volumen != value)
                {
                    _volumen = value;
                    SendPropertyChanged("Volumen");
                }
            }
        }

        private double _wert;
        public double Wert
        {
            get { return _wert; }
            set
            {
                if (_wert != value)
                {
                    _wert = value;
                    SendPropertyChanged("Wert");
                }
            }
        }

        private bool _hasValidValue;
        
        /// <summary>
        /// Indikator, ob Werte einer geladenen Aktie gültig sind
        /// </summary>
        public bool HasValidValue
        {
            get
            {
                return _hasValidValue;
            }
            set
            {
                _hasValidValue = value;
                SendPropertyChanged("HasValidValue");
            }
        }

        /// <summary>
        /// Speichern eines Aktiensymbols im IsolatedStorage
        /// </summary>
        public RelayCommand SaveAktieCommand { get; set; }

        /// <summary>
        /// Laden von Details eines Akteinsymbols aus dem Model
        /// </summary>
        public RelayCommand<string> GetAktieBySymbolCommand { get; set; }

        /// <summary>
        /// Löschen einer Aktie aus dem IsolatedStorage
        /// </summary>
        public RelayCommand<string> DeleteAktieCommand { get; set; }

      /// <summary>
        /// Lädt Informationen einer Aktie anhand ihres Symbols aus dem Model
        /// </summary>
        /// <param name="symbol">Symbol dessen Informationen geladen werden sollen</param>
        public async void GetAktieDetailsBySymbolAsync(string symbol)
        {
            IsBusy = true;

            Aktie aktie = await _aktienStorage
                                .GetAktietBySymbol(symbol);

            if (aktie != null)
            {
                //Eigenschaften des ViewModels initialisieren
                LoadValues(aktie);

                //Speichern des Aktiensymbols aktivieren
                UpdateCanAktieBeSaved(true);
            }
            else
            {
                //Speihern des Aktiensymbols deaktivieren
                UpdateCanAktieBeSaved(false);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Gerufen durch SaveAktieCommand als 'Execute'-Methode
        /// Speichert Aktiensymbol im lokalen Storage
        /// </summary>
        private void Save()
        {
            _aktienStorage.AddAktie(Symbol);
            AppNavigationService.GoBack();
        }

        /// <summary>
        /// Gerufen durch SaveAktieCommand als 'CanExecute'-Methode
        /// </summary>
        public bool CanSave()
        {
            return HasValidValue;
        }
        
        /// <summary>
        /// Fürht 'RaiseCanExecuteChanged'-Event aus
        /// </summary>
        /// <param name="newValue">Indikator, ob das Speichern erlaubt oder nicht erlaubt ist</param>
        private void UpdateCanAktieBeSaved(bool newValue)
        {
            HasValidValue = newValue;
            SaveAktieCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Gerufen durch DeleteAktieCommand als 'Execute'-Methode
        /// </summary>
        /// <param name="symbol">Symbol, das gelöscht werden soll</param>
        public void Delete(string symbol)
        {
            _aktienStorage.DeleteAktie(symbol);
			//Messenger.Default.Send<object>("UpdateListMessage");
			Messenger.Default.Send<object>(null, "UpdateListMessage");
        }

        /// <summary>
        /// Initialisierung der Werte im ViewModel
        /// </summary>
        /// <param name="aktie">neue Werte für das ViewModels</param>
        public void LoadValues(Aktie aktie)
        {
            Symbol = aktie.Symbol;
            Name = aktie.Name;
            ÄnderungNominal = aktie.ÄnderungNominal;
            ÄnderungProzentual = aktie.ÄnderungProzentual;
            Volumen = aktie.Volumen;
            Wert = aktie.Wert;
            Jahreshoch = aktie.Jahreshoch;
            Jahrestief = aktie.Jahrestief;
            Tageshoch = aktie.Tageshoch;
            Tagestief = aktie.Tagestief;
        }

        /// <summary>
        /// bei Instanz-Änderung des NetworkServices
        /// den AktienStorageService neu instanziieren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AktieViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("NetworkService"))
            {
                _aktienStorage = new AktienStorageService();
            }
        }
    }
}
