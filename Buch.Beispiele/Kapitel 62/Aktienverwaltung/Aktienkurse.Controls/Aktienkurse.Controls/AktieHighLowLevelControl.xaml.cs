using System;
using System.Windows;

namespace Aktienkurse.Controls
{
    public partial class AktieHighLowLevelControl
    {
        //Dependency-Eigenshaften
        #region Titel

        public string Titel
        {
            get
            {
                return (string)GetValue(TitelProperty);
            }
            set
            {
                SetValue(TitelProperty, value);
            }
        }

        public static readonly DependencyProperty TitelProperty =
        DependencyProperty.Register("Titel", typeof(string), typeof(AktieHighLowLevelControl), new PropertyMetadata(TitelChangedCallback));

        private static void TitelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieHighLowLevelControl sender = (AktieHighLowLevelControl)d;
            sender.Titel_Text.Text = (string)e.NewValue;
        }

        #endregion

        #region Höchststand

        public double Hoechststand
        {
            get
            {
                return (double)GetValue(HoechststandProperty);
            }
            set { SetValue(HoechststandProperty, value); }
        }

        public static readonly DependencyProperty HoechststandProperty =
        DependencyProperty.Register("Hoechststand", typeof(double), typeof(AktieHighLowLevelControl), new PropertyMetadata(HoechststandChangedCallback));

        private static void HoechststandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieHighLowLevelControl sender = (AktieHighLowLevelControl)d;
            sender.Hoechststand_Text.Text = string.Format("{0:F2}", (double)e.NewValue);
        }

        #endregion

        #region Niedrigststand

        public double Niedrigststand
        {
            get
            {
                return (double)GetValue(NiedrigststandProperty);
            }
            set { SetValue(NiedrigststandProperty, value); }
        }

        public static readonly DependencyProperty NiedrigststandProperty =
        DependencyProperty.Register("Niedrigststand", typeof(double), typeof(AktieHighLowLevelControl), new PropertyMetadata(NiegrigststandChangedCallback));

        private static void NiegrigststandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieHighLowLevelControl sender = (AktieHighLowLevelControl)d;
            sender.Niedrigststand_Text.Text = string.Format("{0:F2}", (double)e.NewValue);
        }

        #endregion

        public AktieHighLowLevelControl()
        {
            InitializeComponent();
        }
    }
}
