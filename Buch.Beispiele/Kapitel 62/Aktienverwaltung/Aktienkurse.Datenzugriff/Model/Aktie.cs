namespace Aktienkurse.DataService.Model
{
    public class Aktie
    {
        public string Name { get; set; }
        public double Wert { get; set; }
        public double Startwert { get; set; }
        public double Endwert { get; set; }
        public double ÄnderungNominal { get; set; }
        public double ÄnderungProzentual { get; set; }
        public double Tagestief { get; set; }
        public double Tageshoch { get; set; }
        public double Jahrestief { get; set; }
        public double Jahreshoch { get; set; }
        public double Volumen { get; set; }
        public string Symbol { get; set; }
    }
}
