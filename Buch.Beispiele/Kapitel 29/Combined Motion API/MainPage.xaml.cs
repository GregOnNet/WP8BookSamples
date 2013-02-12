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

namespace MotionDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        Motion motion;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!Gyroscope.IsSupported)
            {
                MessageBox.Show("Gyroscopde nicht unterstützt!");
            }

            if (!Compass.IsSupported)
            {
                MessageBox.Show("Kompass nicht unterstützt!");
            }

            // Check to see whether the Motion API is supported on the device.
            if (!Motion.IsSupported)
            {
                MessageBox.Show("the Motion API is not supported on this device.");
                return;
            }

            // If the Motion object is null, initialize it and add a CurrentValueChanged
            // event handler.
            if (motion == null)
            {
                motion = new Motion();
                motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);
            }

            // Try to start the Motion API.
            try
            {
                motion.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to start the Motion API.");
            }
        }

        void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            // This event arrives on a background thread. Use BeginInvoke to call
            // CurrentValueChanged on the UI thread.
            Dispatcher.BeginInvoke(() => CurrentValueChanged(e.SensorReading));
        }

        private void CurrentValueChanged(MotionReading e)
        {
            // Check to see if the Motion data is valid.
            if (motion.IsDataValid)
            {
                // Show the numeric values for attitude.
                yawTextBlock.Text = "YAW: " + MathHelper.ToDegrees(e.Attitude.Yaw).ToString("0") + "°";
                pitchTextBlock.Text = "PITCH: " + MathHelper.ToDegrees(e.Attitude.Pitch).ToString("0") + "°";
                rollTextBlock.Text = "ROLL: " + MathHelper.ToDegrees(e.Attitude.Roll).ToString("0") + "°";

                // Set the Angle of the triangle RenderTransforms to the attitude of the device.
                ((RotateTransform)yawtriangle.RenderTransform).Angle = MathHelper.ToDegrees(e.Attitude.Yaw);
                ((RotateTransform)pitchtriangle.RenderTransform).Angle = MathHelper.ToDegrees(e.Attitude.Pitch);
                ((RotateTransform)rolltriangle.RenderTransform).Angle = MathHelper.ToDegrees(e.Attitude.Roll);

                // Show the numeric values for acceleration.
                xTextBlock.Text = "X: " + e.DeviceAcceleration.X.ToString("0.00");
                yTextBlock.Text = "Y: " + e.DeviceAcceleration.Y.ToString("0.00");
                zTextBlock.Text = "Z: " + e.DeviceAcceleration.Z.ToString("0.00");

                // Show the acceleration values graphically.
                xLine.X2 = xLine.X1 + e.DeviceAcceleration.X * 100;
                yLine.Y2 = yLine.Y1 - e.DeviceAcceleration.Y * 100;
                zLine.X2 = zLine.X1 - e.DeviceAcceleration.Z * 50;
                zLine.Y2 = zLine.Y1 + e.DeviceAcceleration.Z * 50;
            }
        }
    }
}