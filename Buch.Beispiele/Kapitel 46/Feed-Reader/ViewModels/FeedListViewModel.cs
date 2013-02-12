using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AsyncFeedReader.Models;

namespace AsyncFeedReader.ViewModels
{
    public class FeedListViewModel : ViewModelBase
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        public FeedListViewModel()
        {
            if (InDesigner)
            {
                return;
            }
            
            LoadFeedAsync2();
        }

        public async Task LoadFeedAsync()
        {
            IsLoading = true;

            var feedClient = new HeiseFeedClient();
            try
            {
                var loadedFeeds = await feedClient.DownloadFeedAsync(_cancellationToken.Token);

                var parsedFeeds = loadedFeeds.Select(f => new FeedEntryViewModel(f));
                Feeds = new ObservableCollection<FeedEntryViewModel>(parsedFeeds);
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.InnerExceptions[0].Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void LoadFeedAsync2()
        {
            IsLoading = true;

            var feedClient = new HeiseFeedClient();
            var loadingFeedTask = feedClient.DownloadFeedAsync(_cancellationToken.Token);
            loadingFeedTask.ContinueWith(t => PopulateViewModel(t.Result), CancellationToken.None,
                                         TaskContinuationOptions.OnlyOnRanToCompletion,
                                         TaskScheduler.FromCurrentSynchronizationContext());
            loadingFeedTask.ContinueWith(HandleProblem, CancellationToken.None,
                                         TaskContinuationOptions.NotOnRanToCompletion,
                                         TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void PopulateViewModel(IEnumerable<FeedEntry> feedEntries)
        {
            var parsedFeeds = feedEntries.Select(f => new FeedEntryViewModel(f));
            Feeds = new ObservableCollection<FeedEntryViewModel>(parsedFeeds);

            IsLoading = false;
        }

        private void HandleProblem(Task<IEnumerable<FeedEntry>> loadingTask)
        {
            if (loadingTask.IsCanceled)
            {
                MessageBox.Show("Download wurde abgebrochen.");
            }
            else
            {
                MessageBox.Show(loadingTask.Exception.InnerExceptions[0].Message);    
            }
            
            IsLoading = false;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ObservableCollection<FeedEntryViewModel> _feeds;

        public ObservableCollection<FeedEntryViewModel> Feeds
        {
            get { return _feeds; }
            set
            {
                if (_feeds == value)
                    return;

                _feeds = value;
                OnPropertyChanged();
            }
        }

        public void CancelLoading()
        {
            _cancellationToken.Cancel();
        }
    }
}