using System.Linq;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class IsoParameter : ArrayParameter<uint?>
    {
        public IsoParameter(PhotoCaptureDevice device) : base("ISO", KnownCameraPhotoProperties.Iso, device) {}

        protected override void PopulateOptions()
        {
            var auto = new ArrayParameterOption<uint?>(null, "Auto");
            Options.Add(auto);
            SelectedOption = auto;

            CameraCapturePropertyRange range = PhotoCaptureDevice.GetSupportedPropertyRange(Device.SensorLocation,
                                                                                            ParameterId);
            object currentValue = Device.GetProperty(ParameterId);
            uint[] standardValues = {100, 200, 400, 800, 1600, 3200};

            var min = (uint)range.Min;
            var max = (uint)range.Max;

            foreach (uint isoValue in standardValues)
            {
                if (isoValue >= min && isoValue <= max)
                {
                    string name = string.Format("ISO {0}", isoValue);
                    var option = new ArrayParameterOption<uint?>(isoValue, name);
                    Options.Add(option);

                    if (isoValue.Equals(currentValue))
                    {
                        SelectedOption = option;
                    }
                }
            }
        }

        protected override void SetOption(ArrayParameterOption<uint?> option)
        {
            Device.SetProperty(ParameterId, option.Value);
        }

        public override void SetDefaultValue()
        {
            SelectedOption = Options.FirstOrDefault();
        }
    }
}