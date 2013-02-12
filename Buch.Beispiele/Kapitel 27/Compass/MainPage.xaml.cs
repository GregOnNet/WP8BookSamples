using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;

namespace CompassDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Compass compass;
        private bool isCalibrating = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!Compass.IsSupported)
            {
                MessageBox.Show("Dein Gerät unterstützt keinen Kompass :(");
                return;
            }
            
            compass = new Compass();
            compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(500);
            compass.CurrentValueChanged += compass_CurrentValueChanged;
            compass.Calibrate += compass_Calibrate;
            compass.Start();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Ressourcen freigeben
            if (compass != null)
            {
                compass.Stop();
                compass.CurrentValueChanged -= compass_CurrentValueChanged;
                compass.Dispose();
            }            
        }

        private void compass_Calibrate(object sender, CalibrationEventArgs e)
        {
            isCalibrating = true;
            Dispatcher.BeginInvoke(UpdateCalibrateUI);
        }

        private void UpdateCalibrateUI()
        {
            if (isCalibrating)
            {
                Calibrate.Text = "JA!";
            }
            else
            {
                Calibrate.Text = "Nein.";
            }
        }

        private void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            if (isCalibrating)
            {
                if (e.SensorReading.HeadingAccuracy < 20.0)
                {
                    isCalibrating = false;
                    Dispatcher.BeginInvoke(UpdateCalibrateUI);
                }
            }
            else
            {
                Dispatcher.BeginInvoke(() => UpdateDataUI(e.SensorReading));
            }
        }
        
        private void UpdateDataUI(CompassReading data)
        {
            HeadingMagnN.Text = data.MagneticHeading.ToString() + "°";
            HeadingGeoN.Text = data.TrueHeading.ToString() + "°";
            Accuracy.Text = data.HeadingAccuracy.ToString() + "°";
        }
    }
}