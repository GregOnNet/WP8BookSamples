using System;
using System.Diagnostics;

namespace AsyncFeedReader.Models
{
    /// <summary>
    /// Stores the information of a single feed entry.
    /// </summary>
    [DebuggerDisplay("{Title} (vom {PublishedOn})")]
    public class FeedEntry
    {
        public FeedEntry(string title, Uri feedUri, DateTime publishedOn, DateTime updatedOn, string summary)
        {
            Title = title;
            FeedUri = feedUri;
            PublishedOn = publishedOn;
            UpdatedOn = updatedOn;
            Summary = summary;
        }

        public string Title { get; private set; }
        public Uri FeedUri { get; private set; }
        public DateTime PublishedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        public string Summary { get; private set; }
    }
}