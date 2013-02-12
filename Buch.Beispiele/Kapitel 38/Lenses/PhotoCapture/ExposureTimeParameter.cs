using System.Linq;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class ExposureTimeParameter : ArrayParameter<uint?>
    {
        public ExposureTimeParameter(PhotoCaptureDevice device)
            : base("Belichtungszeit", KnownCameraPhotoProperties.ExposureTime, device) {}

        protected override void PopulateOptions()
        {
            var auto = new ArrayParameterOption<uint?>(null, "Auto");
            Options.Add(auto);

            CameraCapturePropertyRange times = PhotoCaptureDevice.GetSupportedPropertyRange(Device.SensorLocation,
                                                                                            KnownCameraPhotoProperties
                                                                                                .ExposureTime);
            var currentValue = (uint?)Device.GetProperty(ParameterId);

            uint[] standardValues = {2000, 1000, 500, 250, 125, 60, 30, 15, 8, 4, 2, 1};
            var min = (uint)times.Min;
            var max = (uint)times.Max;

            foreach (uint timeValue in standardValues)
            {
                uint microSeconds = 1000000 / timeValue;

                if (microSeconds >= min && microSeconds <= max)
                {
                    string name = string.Format("1 / {0} s", timeValue);
                    var option = new ArrayParameterOption<uint?>(microSeconds, name);
                    Options.Add(option);

                    if (currentValue == microSeconds)
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