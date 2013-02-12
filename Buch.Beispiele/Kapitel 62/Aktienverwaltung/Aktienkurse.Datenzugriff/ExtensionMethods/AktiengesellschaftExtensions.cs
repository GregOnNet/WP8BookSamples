using Aktienkurse.DataService.Model;

namespace Aktienkurse.DataService.ExtensionMethods
{
    public static class AktiengesellschaftExtensions
    {
        public static bool IsValid(this Aktie aktienGesellschaft)
        {
            bool isInvalid = double.IsNaN(aktienGesellschaft.Startwert) &&
                 double.IsNaN(aktienGesellschaft.Endwert) &&
                 double.IsNaN(aktienGesellschaft.ÄnderungNominal) &&
                 double.IsNaN(aktienGesellschaft.ÄnderungProzentual) &&
                 double.IsNaN(aktienGesellschaft.Tagestief) &&
                 double.IsNaN(aktienGesellschaft.Tageshoch) &&
                 double.IsNaN(aktienGesellschaft.Jahrestief) &&
                 double.IsNaN(aktienGesellschaft.Jahreshoch) &&
                 double.IsNaN(aktienGesellschaft.Volumen);

            return !isInvalid;
        }
    }
}
