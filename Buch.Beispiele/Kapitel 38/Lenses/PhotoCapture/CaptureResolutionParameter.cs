using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class CaptureResolutionParameter : ArrayParameter<Size>
    {
        public CaptureResolutionParameter(PhotoCaptureDevice device) : base("Kameraauflösung", Guid.Empty, device) {}

        protected override void PopulateOptions()
        {
            IReadOnlyList<Size> cameraResolutions =
                PhotoCaptureDevice.GetAvailableCaptureResolutions(Device.SensorLocation);
            Size currentResolution = Device.CaptureResolution;

            foreach (Size resolution in cameraResolutions)
            {
                string optionName = string.Format("{0}x{1}", resolution.Width, resolution.Height);
                var option = new ArrayParameterOption<Size>(resolution, optionName);

                Options.Add(option);

                if (resolution == currentResolution)
                {
                    SelectedOption = option;
                }
            }
        }

        protected override async void SetOption(ArrayParameterOption<Size> option)
        {
            if (IsModifiable)
            {
                IsModifiable = false;

                await Device.SetCaptureResolutionAsync(option.Value);

                IsModifiable = true;
            }
        }

        public override void SetDefaultValue()
        {
            SelectedOption = Options.FirstOrDefault();
        }
    }
}