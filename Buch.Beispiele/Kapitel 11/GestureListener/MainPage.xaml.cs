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

namespace GestureListener
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void GestureListener_DoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            MessageBox.Show("Zwei Mal getippt.");
        }

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (e.VerticalVelocity < 0)
            {
                MessageBox.Show("Nach oben geschnippst");
            }
            else
            {
                MessageBox.Show("Nach unten geschnippst");
            }
        }

        private void GestureListener_Hold(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            MessageBox.Show("Finger verweilt");
        }

        
    }
}