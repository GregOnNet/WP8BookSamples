using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FirstUserControl.Controls
{
    public class WatermarkTextBox : TextBox
    {
        public string WatermarkText
        {
            get { return (string)GetValue(WaterMarkTextProperty); }
            set { SetValue(WaterMarkTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterMarkText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterMarkTextProperty =
            DependencyProperty.Register("WatermarkText",
                                        typeof(string), typeof(WatermarkTextBox), null);

        public WatermarkTextBox()
            : base()
        {
            Loaded += (s, e) =>
                {
                    SetWaterMark();

                    LostFocus += new RoutedEventHandler(TrySetWatermark);
                    Tap += new EventHandler<GestureEventArgs>(SelectWholeText);
                };
        }

        void SelectWholeText(object sender, GestureEventArgs e)
        {
            SetNormalStyle();
            SelectAll();
        }

        private void TrySetWatermark(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Text) ||
                Text.Equals(WatermarkText))
            {
                SetWaterMark();
            }
        }

        private void SetWaterMark()
        {
            SetWatermarkStyle();
            Text = WatermarkText;
        }
        //Styles
        private void SetWatermarkStyle()
        {
            FontStyle = FontStyles.Italic;
            Background = new SolidColorBrush(Colors.Red);
            Foreground = new SolidColorBrush(Colors.White);
        }
        
        private void SetNormalStyle()
        {
            FontStyle = FontStyles.Normal;
            Background = new SolidColorBrush(Colors.LightGray);
            Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
