using System.Windows;
using AsyncFeedReader.ViewModels;
using Windows.System;

namespace AsyncFeedReader.Views
{
    public partial class FeedsPage
    {
        public FeedsPage()
        {
            InitializeComponent();
        }

        private void Feed_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;
            var feedEntry = (FeedEntryViewModel)frameworkElement.DataContext;
            Launcher.LaunchUriAsync(feedEntry.FeedUri);
        }

        private void CancelLoading_Click(object sender, System.EventArgs e)
        {
            var vm = (FeedListViewModel)DataContext;
            vm.CancelLoading();
        }
    }
}