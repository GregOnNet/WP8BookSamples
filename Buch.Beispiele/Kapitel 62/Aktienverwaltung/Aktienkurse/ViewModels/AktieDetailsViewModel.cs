using GalaSoft.MvvmLight.Messaging;

namespace Aktienkurse.ViewModels
{
    public class AktieDetailsViewModel : AktieViewModel
    {
        public AktieDetailsViewModel()
            : base()
        {
            //Registrierung an Message, die das Laden von Details einer Aktie auslöst
            Messenger.Default.Register<string>(this, "LoadAktieDetailsBySymbolMessage", GetAktieDetailsBySymbolAsync);
        }
    }
}
