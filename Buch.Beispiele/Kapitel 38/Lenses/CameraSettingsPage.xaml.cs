using System.Linq;
using System.Windows.Navigation;
using BlackWhiteImageStyler.PhotoCapture;

namespace BlackWhiteImageStyler
{
    public partial class CameraSettingsPage
    {
        public CameraSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SceneOptions.DataContext = App.Camera.Parameters.FirstOrDefault(p => p is SceneModeParameter);
            CaptureResolution.DataContext = App.Camera.Parameters.FirstOrDefault(p => p is CaptureResolutionParameter);
            IsoOptions.DataContext = App.Camera.Parameters.FirstOrDefault(p => p is IsoParameter);
            ExposureTimeOptions.DataContext = App.Camera.Parameters.FirstOrDefault(p => p is ExposureTimeParameter);

            var exposureCompensation = App.Camera.Parameters.FirstOrDefault(p => p is ExposureCompensationParameter);
            ExposureCompensation.SetParameter((ExposureCompensationParameter)exposureCompensation);
        }
    }
}