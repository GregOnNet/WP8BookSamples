using System.Windows;
using BlackWhiteImageStyler.PhotoCapture;

namespace BlackWhiteImageStyler.Controls
{
    public partial class RangeParameterOptions
    {
        public RangeParameterOptions()
        {
            InitializeComponent();
        }

        private ExposureCompensationParameter _exposureCompensation;
        public void SetParameter(ExposureCompensationParameter parameter)
        {
            _exposureCompensation = parameter;

            ValueSlider.Minimum = _exposureCompensation.Minimum;
            ValueSlider.Maximum = _exposureCompensation.Maximum;
            ValueSlider.Value = _exposureCompensation.Value;
        }

        private void ValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_exposureCompensation != null)
            {
                _exposureCompensation.Value = (int)e.NewValue;
            }
        }
    }
}
