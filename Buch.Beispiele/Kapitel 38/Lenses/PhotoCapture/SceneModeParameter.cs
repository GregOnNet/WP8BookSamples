using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class SceneModeParameter : ArrayParameter<CameraSceneMode>
    {
        public SceneModeParameter(PhotoCaptureDevice device)
            : base("Bildmodus", KnownCameraPhotoProperties.SceneMode, device) {}

        protected override void PopulateOptions()
        {
            IReadOnlyList<object> supportedValues = PhotoCaptureDevice.GetSupportedPropertyValues(
                Device.SensorLocation, ParameterId);
            object currentValue = Device.GetProperty(ParameterId);

            foreach (dynamic mode in supportedValues)
            {
                var csm = (CameraSceneMode)mode;
                var option = new ArrayParameterOption<CameraSceneMode>(csm, Enum.GetName(typeof(CameraSceneMode), mode));
                Options.Add(option);

                if (mode.Equals(currentValue))
                {
                    SelectedOption = option;
                }
            }
        }

        protected override void SetOption(ArrayParameterOption<CameraSceneMode> option)
        {
            Device.SetProperty(ParameterId, option.Value);
        }

        public override void SetDefaultValue()
        {
            SelectedOption = Options.First();
        }
    }
}