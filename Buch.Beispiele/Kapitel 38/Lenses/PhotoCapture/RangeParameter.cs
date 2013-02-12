using System;
using System.Diagnostics;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class RangeParameter<T> : Parameter
    {
        private T _value;

        public RangeParameter(string name, Guid parameterId, PhotoCaptureDevice device)
            : base(name, parameterId, device)
        {
            ReadCurrent();
        }

        public T Minimum { get; private set; }
        public T Maximum { get; private set; }

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (!_value.Equals(value))
                {
                    try
                    {
                        _value = value;
                        Device.SetProperty(ParameterId, value);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Failed to set {0}", Name);
                    }
                }
            }
        }

        public override sealed void ReadCurrent()
        {
            try
            {
                CameraCapturePropertyRange range = PhotoCaptureDevice.GetSupportedPropertyRange(Device.SensorLocation,
                                                                                                ParameterId);
                if (range == null)
                {
                    IsSupported = false;
                }
                else
                {
                    Minimum = (T)range.Min;
                    Maximum = (T)range.Max;
                    _value = (T)Device.GetProperty(ParameterId);
                    IsSupported = true;
                }
            }
            catch (Exception)
            {
                IsSupported = false;
            }

            IsModifiable = IsSupported && !Minimum.Equals(Maximum);
        }

        public override void SetDefaultValue()
        {
            Value = Minimum;
        }
    }
}