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
    public class LabeledWaternarkTextBox : LabeledTextBox
    {
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark",
                                        typeof(string),
                                        typeof(LabeledWaternarkTextBox),
                                        null);

        public LabeledWaternarkTextBox()
            :base()
        {
            Loaded += (s, e) =>
            {
                SetWaterMark();

                LostFocus += new RoutedEventHandler
                                        (TrySetWatermark);
                Tap += new EventHandler<GestureEventArgs>
                                        (SelectWholeText);
            };
        }

        private void SelectWholeText(object sender,
                                            GestureEventArgs e)
        {
            var textBox = sender as LabeledWaternarkTextBox;
            SetNormalStyle();

            textBox.InhaltsText.SelectAll();
        }

        private void TrySetWatermark(object sender,
                                     RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Text) ||
                Text.Equals(Watermark))
            {
                SetWaterMark();
            }
        }

        private void SetWaterMark()
        {
            SetWatermarkStyle();
            InhaltsText.Text = Watermark;
        }

        private void SetWatermarkStyle()
        {
            InhaltsText.FontStyle = FontStyles.Italic;
            InhaltsText.Background =
                new SolidColorBrush(Colors.DarkGray);
            InhaltsText.Foreground =
                new SolidColorBrush(Colors.Black);
        }

        private void SetNormalStyle()
        {
            InhaltsText.FontStyle = FontStyles.Normal;
            InhaltsText.Background =
                new SolidColorBrush(Colors.LightGray);
            InhaltsText.Foreground =
                new SolidColorBrush(Colors.Black);
        }
    }
}
