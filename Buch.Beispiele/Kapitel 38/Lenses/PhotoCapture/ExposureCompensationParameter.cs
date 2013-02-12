using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class ExposureCompensationParameter : RangeParameter<int>
    {
        public ExposureCompensationParameter(PhotoCaptureDevice device) :
            base("Belichtungskorrektur", KnownCameraPhotoProperties.ExposureCompensation, device) {}

        public override void SetDefaultValue()
        {
            Value = (int)(Minimum + (Maximum - Minimum) / 2.0);
        }
    }
}