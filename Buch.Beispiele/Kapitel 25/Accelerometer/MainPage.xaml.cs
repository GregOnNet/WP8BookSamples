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

namespace AccelerometerDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        Accelerometer sensor;
        private double minLeft;
        private double maxLeft;
        private double minTop;
        private double maxTop;
        private double ballX;
        private double ballY;

        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            minLeft = 0;
            minTop = 0;
            maxLeft = BallPanel.ActualWidth - Ball.Width;
            maxTop = BallPanel.ActualHeight - Ball.Height;

            ballX = maxLeft / 2.0;
            ballY = maxTop / 2.0;

            Canvas.SetLeft(Ball, ballX);
            Canvas.SetTop(Ball, ballY);

            sensor = new Accelerometer();
            sensor.TimeBetweenUpdates = TimeSpan.FromMilliseconds(50);
            sensor.CurrentValueChanged += sensor_CurrentValueChanged;
            sensor.Start();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            // Ressourcen freigeben
            sensor.Stop();
            sensor.CurrentValueChanged -= sensor_CurrentValueChanged;
            sensor.Dispose();
        }

        private void sensor_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => UpdateBall(e.SensorReading.Acceleration));
        }

        private void UpdateBall(Vector3 data)
        {
            ballX += data.X * BallPanel.ActualWidth;
            ballY += data.Y * BallPanel.ActualHeight;

            if (ballX < minLeft)
                ballX = minLeft;

            if (ballX > maxLeft)
                ballX = maxLeft;

            if (ballY < minTop)
                ballY = minTop;

            if (ballY > maxTop)
                ballY = maxTop;

            Canvas.SetLeft(Ball, ballX);
            Canvas.SetTop(Ball, ballY);
        }
    }
}