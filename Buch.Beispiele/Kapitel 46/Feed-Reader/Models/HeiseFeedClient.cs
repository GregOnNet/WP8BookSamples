using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AsyncFeedReader.Models
{
    public class HeiseFeedClient
    {
        private static readonly Uri FeedUri = new Uri("http://www.heise.de/newsticker/heise-atom.xml");
        private static readonly XNamespace AtomNs = "http://www.w3.org/2005/Atom";

        public async Task<IEnumerable<FeedEntry>> DownloadFeedAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Enumerable.Empty<FeedEntry>();
            }

            var downloadedFeed = await DownloadFeedXmlAsync();

            // Abbrechen, wenn angefordert
            cancellationToken.ThrowIfCancellationRequested();

            var parsedFeed = await ParseFeedAsync(downloadedFeed);
            return parsedFeed;
        }
        
        private async Task<string> DownloadFeedXmlAsync()
        {
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            return await client.DownloadStringTaskAsync(FeedUri);
        }

        private Task<IEnumerable<FeedEntry>> ParseFeedAsync(string xmlFeed)
        {
            return Task.Run(() => ParseFeed(xmlFeed));
        }

        private IEnumerable<FeedEntry> ParseFeed(string xmlFeed)
        {
            // Atom Feed in XDocument laden und parsen
            var feed = XDocument.Parse(xmlFeed);

            // Alle Feed-Einträge auslesen parsen
            var parsedEntries = from entry in feed.Descendants(AtomNs + "entry")
                                let title = entry.Element(AtomNs + "title").Value
                                let feedUrl = new Uri(entry.Element(AtomNs + "id").Value)
                                let publishedOn = DateTime.Parse(entry.Element(AtomNs + "published").Value)
                                let updatedOn = DateTime.Parse(entry.Element(AtomNs + "updated").Value)
                                let summary = entry.Element(AtomNs + "summary").Value
                                select new FeedEntry(title, feedUrl, publishedOn, updatedOn, summary);
            
            // Ergebnis als Liste zurückgeben, wodurch jetzt das Parsen durchgeführt wird und nicht beim ersten Zugriff
            return parsedEntries.ToList();
        }
    }
}