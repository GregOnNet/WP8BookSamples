using InAppPurchase.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace InAppPurchase.Views
{
    public partial class FilterManipulationView : PhoneApplicationPage
    {
        private readonly PhotoChooserTask _photoChooser;
        private readonly FilterManipulation _vm;

        public FilterManipulationView()
        {
            InitializeComponent();

            _vm = (FilterManipulation)DataContext;
            _photoChooser = new PhotoChooserTask
            {
                ShowCamera = true
            };
            _photoChooser.Completed += PhotoChooser_Completed;
        }

        private void PhotoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var selectedImage = new BitmapImage();
                selectedImage.SetSource(e.ChosenPhoto);
                _vm.OriginalImage = selectedImage;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _vm.LoadFilters();
        }

        private void SelectImage_Click(object sender, EventArgs e)
        {
            _photoChooser.Show();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            _vm.Save();
        }

        private void FilterParameter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _vm.Apply();
        }
    }
}