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
using Microsoft.Xna.Framework;

namespace GyroscopeDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Gyroscope gyro;
        private DateTimeOffset lastTimestamp;
        private Vector3 rotation;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!Gyroscope.IsSupported)
            {
                MessageBox.Show("Ihr Gerät verfügt über keinem Gyroskope!");
                return;
            }

            lastTimestamp = DateTimeOffset.MinValue;
            gyro = new Gyroscope();
            gyro.CurrentValueChanged += gyro_CurrentValueChanged;
            gyro.TimeBetweenUpdates = TimeSpan.FromMilliseconds(100);
            gyro.Start();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (gyro != null)
            {
                gyro.Stop();
                gyro.CurrentValueChanged -= gyro_CurrentValueChanged;
            }
        }

        private void gyro_CurrentValueChanged(object sender, SensorReadingEventArgs<GyroscopeReading> e)
        {
            if (lastTimestamp == DateTimeOffset.MinValue)
            {
                lastTimestamp = e.SensorReading.Timestamp;
            }
            else
            {
                TimeSpan difference = e.SensorReading.Timestamp - lastTimestamp;
                rotation = e.SensorReading.RotationRate * (float)difference.TotalSeconds;
                lastTimestamp = e.SensorReading.Timestamp;
                Dispatcher.BeginInvoke(() => UpdateUI(rotation));    
            }
        }

        private void UpdateUI(Vector3 rotation)
        {
            X.Text = rotation.X.ToString("0.00");
            XD.Text = MathHelper.ToDegrees(rotation.X).ToString("0.00");

            Y.Text = rotation.Y.ToString("0.00");
            YD.Text = MathHelper.ToDegrees(rotation.Y).ToString("0.00");

            Z.Text = rotation.Y.ToString("0.00");
            ZD.Text = MathHelper.ToDegrees(rotation.Z).ToString("0.00");
        }
    }
}