using InAppPurchase.Filters;
using Microsoft.Xna.Framework.Media;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InAppPurchase.ViewModels
{
    public class FilterManipulation : ViewModelBase
    {
        public void LoadFilters()
        {
            var bw = new ImageFilter(new BlackAndWhite());
            var sepia = new ImageFilter(new Sepia()) { Value = 20 };
            var brightness = new ImageFilter(new Brightness()) { Value = 20 };
            var contrast = new ImageFilter(new Contrast()) { Value = 20 };

            var filters = new ObservableCollection<ImageFilter> { bw, sepia };

            bool isBrightnessBought;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(InAppProducts.BrightnessFilterIdentifier,
                                                                        out isBrightnessBought))
            {
                if (isBrightnessBought)
                {
                    filters.Add(brightness);
                }
            }
            bool isContrastBought;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(InAppProducts.ContrastFilterIdentifier,
                                                                        out isContrastBought))
            {
                if (isContrastBought)
                {
                    filters.Add(contrast);
                }
            }

            Filters = filters;
            SelectedFilter = Filters.First();
        }

        private ObservableCollection<ImageFilter> _filters;

        public ObservableCollection<ImageFilter> Filters
        {
            get { return _filters; }
            set
            {
                if (_filters == value)
                    return;

                _filters = value;
                SendPropertyChanged();
            }
        }

        private ImageFilter _selectedFilter;

        public ImageFilter SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (_selectedFilter == value)
                    return;

                _selectedFilter = value;
                SendPropertyChanged();

                Apply();
            }
        }

        private BitmapImage _originalImage;

        public BitmapImage OriginalImage
        {
            get { return _originalImage; }
            set
            {
                if (_originalImage == value)
                    return;

                _originalImage = value;
                SendPropertyChanged();

                Apply();
            }
        }

        private WriteableBitmap _editImage;

        public WriteableBitmap EditImage
        {
            get { return _editImage; }
            set
            {
                if (_editImage == value)
                    return;

                _editImage = value;
                SendPropertyChanged();
            }
        }

        public async void Apply()
        {
            if (OriginalImage == null)
            {
                return;
            }

            var copy = new WriteableBitmap(OriginalImage);

            await Task.Run(() => SelectedFilter.Filter.Apply(copy));

            EditImage = copy;
        }

        public void Save()
        {
            using (var library = new MediaLibrary())
            {
                using (var saveStream = new MemoryStream())
                {
                    EditImage.SaveJpeg(saveStream, EditImage.PixelWidth, EditImage.PixelHeight, 0, 85);
                    saveStream.Position = 0;

                    library.SavePicture("Image Filters", saveStream);
                }
            }
        }
    }
}
