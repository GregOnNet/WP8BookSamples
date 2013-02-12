using System.ComponentModel;
using Phone.Helpers.Interfaces;
using Phone.Helpers.Services;

namespace Aktienkurse.ViewModels
{
    public class BasisViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        
        /// <summary>
        /// Indikator, ob ViewModel Daten lädt oder andere
        /// zeitaufwendige Aktionen asynchron ausfürht.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                SendPropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Dienst zur Feststellung der Netzwekkonnektivität
        /// </summary>
        private INetworkService _networkService;
        public INetworkService NetworkService
        {
            get
            {
                if (_networkService == null)
                {
                    _networkService = new NetworkService();
                }
                return _networkService;
            }
            set
            {
                _networkService = value;
                SendPropertyChanged("NetworkService");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Löst das 'PropertyChanged'-Event für eine Eigenschaft aus, wodurch die Datenbindung
        /// der sich abonnierenden View aktualisiert werden.
        /// </summary>
        /// <param name="propertyName">Name der geänderten Eigenschaft</param>
        protected void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}