using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DataBinding.Controls
{
    public partial class PictureListItemControl : UserControl
    {
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(PictureListItemControl), new PropertyMetadata(DescriptionCallback));

        public static void DescriptionCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PictureListItemControl pic = sender as PictureListItemControl;
            pic.txt_Description.Text = e.NewValue.ToString();
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(PictureListItemControl), new PropertyMetadata(ImageChanged));

        public static void ImageChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PictureListItemControl control = (PictureListItemControl)sender;
            BitmapImage image = (BitmapImage)e.NewValue;

            control.img_DisplayImage.Source = image as ImageSource;
        }
        
        public string ImageInformation
        {
            get { return (string)GetValue(ImageInformationProperty); }
            set { SetValue(ImageInformationProperty, value); }
        }
        
        public static DependencyProperty ImageInformationProperty =
            DependencyProperty.Register("ImageInformation", typeof(string), typeof(PictureListItemControl), new PropertyMetadata(ImageInformationChanged));

        public static void ImageInformationChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PictureListItemControl control = (PictureListItemControl)sender;
            control.txt_ImageInformation.Text = e.NewValue.ToString();
        }

        public PictureListItemControl()
        {
            InitializeComponent();
        }
    }
}
