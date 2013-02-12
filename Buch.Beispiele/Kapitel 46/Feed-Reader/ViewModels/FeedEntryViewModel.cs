using System;
using System.Diagnostics;
using System.Windows;
using AsyncFeedReader.Models;

namespace AsyncFeedReader.ViewModels
{
    [DebuggerDisplay("{Title} (vom {PublishedOn})")]
    public class FeedEntryViewModel : ViewModelBase
    {
        public FeedEntryViewModel() { }

        public FeedEntryViewModel(FeedEntry feedEntry)
        {
            Title = feedEntry.Title;
            FeedUri = feedEntry.FeedUri;
            Summary = feedEntry.Summary;
            PublishedOn = feedEntry.PublishedOn;
            UpdatedOn = feedEntry.UpdatedOn;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;

                _title = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
        private Uri _feedUri;

        public Uri FeedUri
        {
            get { return _feedUri; }
            set
            {
                if (_feedUri == value)
                    return;

                _feedUri = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _summary;

        public string Summary
        {
            get { return _summary; }
            set
            {
                if (_summary == value)
                    return;

                _summary = value;
                OnPropertyChanged();
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _publishedOn;

        public DateTime PublishedOn
        {
            get { return _publishedOn; }
            set
            {
                if (_publishedOn == value)
                    return;

                _publishedOn = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _updatedOn;

        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set
            {
                if (_updatedOn == value)
                    return;

                _updatedOn = value;
                OnPropertyChanged();
                OnPropertyChanged("UpdatedOnVisibility");
            }
        }

        public Visibility UpdatedOnVisibility
        {
            get 
            { 
                var difference = UpdatedOn - PublishedOn;
                return difference.TotalMinutes > 5 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}