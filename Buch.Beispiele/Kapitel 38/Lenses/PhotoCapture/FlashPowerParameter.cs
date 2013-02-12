using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class FlashPowerParameter : RangeParameter<uint>
    {
        public FlashPowerParameter(PhotoCaptureDevice device) :
            base("Blitzintensität", KnownCameraPhotoProperties.FlashPower, device) {}

        public override void SetDefaultValue()
        {
            Value = (uint)(Minimum + (Maximum - Minimum) / 2.0);
        }
    }
}