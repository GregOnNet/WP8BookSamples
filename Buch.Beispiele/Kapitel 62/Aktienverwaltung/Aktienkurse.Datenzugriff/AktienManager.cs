using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aktienkurse.DataService.ExtensionMethods;
using Aktienkurse.DataService.Model;

namespace Aktienkurse.DataService
{
    /// <summary>
    /// Zugriffslogik für das Laden von Aktieninformationen
    /// unter Verwendung von 'Yahoo! Finanzen'
    /// http://de.finance.yahoo.com/
    /// </summary>
    public class AktienManager : IAktienManager
    {
        // Aktienwerte liegen im englischen Format vor
        private static CultureInfo _parsingLanguage = new CultureInfo("en-US");

        /// <summary>
        /// Lädt eine Liste von Akteingeselschaften
        /// </summary>
        /// <param name="symbole">Liste von Aktiensymbolen die geladen werden sollen</param>
        /// <returns>Liste von Aktiengesellschaften mit aktuellen Kursinformationen.</returns>
        public async Task<List<Aktie>> GetAktienBySymbole(params string[] symbole)
        {
            if (symbole == null || symbole.Length == 0)
            {
                throw new ArgumentException
                    ("Es wurden keine Aktiensymbole angegeben!", "symbole");
            }

            Uri quoteUri = GenerateAktiengesellschaftRequestUri(symbole);

            // CSV downloaden
            var client = new WebClient();
            string aktiengesellschaftenCsv = await client.DownloadStringTaskAsync(quoteUri);

            // CSV parsen
            var result = await Task.Run<List<Aktie>>(() => ParseAktienCsv(aktiengesellschaftenCsv));

            for (int i = 0; i < symbole.Length; i++)
            {
                result[i].Symbol = symbole[i];
            }

            // Ergebnis zurückliefern
            return result;
        }

        /// <summary>
        ///  Akteingeselschaft mit aktuellen Kursinformationen
        /// </summary>
        /// <param name="symbol">Liste von Aktiensymbolen die geladen werden sollen</param>
        /// <returns>Aktiengesellschaft mit aktuellen Kursinformationen.</returns>
        public async Task<Aktie> GetAktieBySymbol(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentException("Es wurde kein Aktiensymbol angegeben!", "symbol");
            }

            Uri quoteUri = GenerateAktiengesellschaftRequestUri(new string[] { symbol });

            // CSV downloaden
            var client = new WebClient();
            string aktienCsv = await client.DownloadStringTaskAsync(quoteUri);

            // CSV parsen
            var result = await Task.Run<List<Aktie>>(() => ParseAktienCsv(aktienCsv));

            if (result != null && result.Count == 1)
            {
                Aktie aktie = result[0];
                aktie.Symbol = symbol;

                return aktie;
            }
            // Ergebnis zurückliefern
            return null;
        }

        /// <summary>
        /// Erzeugt Request-Uri für den Webservice von Yahoo
        /// </summary>
        /// <param name="symbole">Array von Symbolen von denen Kursinformationen geladen werden sollen.</param>
        /// <returns>Request-Uri</returns>
        private Uri GenerateAktiengesellschaftRequestUri(string[] symbole)
        {
            var requestUriBuilder = new StringBuilder();

            // Basis-URL
            requestUriBuilder.Append("http://finance.yahoo.com/d/aktien.csv?s=");

            // Symbole hinzufügen
            for (int i = 0; i < symbole.Length; i++)
            {
                requestUriBuilder.Append(symbole[i]);
                if (i != symbole.Length - 1)
                {
                    requestUriBuilder.Append("+");
                }
            }

            // Symbolkombination festlegen
            // Legt fest, welche Werte abgefragt werden
            // Alle Symbole sind z.B. unter http://www.gummy-stuff.org/Yahoo-data.htm abrufbar
            requestUriBuilder.Append("&f=nl1poc1p2ghjkv");

            string finaleUrl = requestUriBuilder.ToString();
            return new Uri(finaleUrl, UriKind.Absolute);
        }

        /// <summary>
        /// Wertet Antwort des Yahoo Webdienstes
        /// </summary>
        /// <param name="csv">auszwertende CSV-Datei als string</param>
        /// <returns>Liste von Aktiengesellschaften</returns>
        private List<Aktie> ParseAktienCsv(string csv)
        {
            // Aktienkursinformationen in Liste speichern
            var aktienList = new List<Aktie>();

            // Geparse Aktieninfos werden in Array zwischengespeichert
            var werte = new double[10];

            using (var valueStream = new StringReader(csv))
            {
                while (valueStream.Peek() > -1)
                {
                    var aktie = new Aktie();

                    // Zeile aufteilen
                    string[] csvWerte = valueStream.ReadLine().Split(',');

                    if (string.IsNullOrWhiteSpace(csvWerte[0]))
                    {
                        continue;
                    }

                    for (int i = 0; i < csvWerte.Length; i++)
                    {
                        // Wert normalisieren (" und % entfernen)
                        string normalisierterText = csvWerte[i].Replace("\"", "")
                                                               .Replace("%", "");

                        if (i == 0)
                        {
                            // 1. Wert ist der Name der Aktie
                            aktie.Name = normalisierterText;
                        }
                        else
                        {
                            // Alle weiteren Werte sind vom Typ double
                            // Wert parsen nach englischen Format parsen
                            if (normalisierterText == "N/A")
                            {
                                // Es steht kein Wert zur Verfügung
                                werte[i - 1] = double.NaN;
                            }
                            else
                            {
                                double value = double.Parse(normalisierterText, NumberStyles.Currency, _parsingLanguage);
                                werte[i - 1] = value;
                            }
                        }
                    }

                    // Wertzuweisung in Abstimmung mit Symbolkombination
                    aktie.Wert = werte[0];
                    aktie.Endwert = werte[1];
                    aktie.Startwert = werte[2];
                    aktie.ÄnderungNominal = werte[3];
                    aktie.ÄnderungProzentual = werte[4];
                    aktie.Tagestief = werte[5];
                    aktie.Tageshoch = werte[6];
                    aktie.Jahrestief = werte[7];
                    aktie.Jahreshoch = werte[8];
                    aktie.Volumen = werte[9];

                    // Geparste Aktieninfos dem Ergebnis hinzufügen
                    if (aktie.IsValid())
                    {
                        aktienList.Add(aktie);
                    }
                }
            }

            return aktienList;
        }
    }
}