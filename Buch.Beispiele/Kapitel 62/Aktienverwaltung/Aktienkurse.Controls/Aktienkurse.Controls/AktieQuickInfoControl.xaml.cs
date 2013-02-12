using System.Windows;

namespace Aktienkurse.Controls
{
    public partial class AktieQuickInfoControl
    {
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
        DependencyProperty.Register("Titel", typeof(string), typeof(AktieQuickInfoControl), new PropertyMetadata(TitelChangedCallback));

        public static void TitelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieQuickInfoControl sender = (AktieQuickInfoControl)d;
            sender.Titel_Text.Text = (string)e.NewValue;
        }

        #endregion

        #region Symbol

        public string Symbol
        {
            get
            {
                return (string)GetValue(SymbolProperty);
            }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }

        public static readonly DependencyProperty SymbolProperty =
        DependencyProperty.Register("Symbol", typeof(string), typeof(AktieQuickInfoControl), new PropertyMetadata(SymbolChangedCallback));

        public static void SymbolChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieQuickInfoControl sender = (AktieQuickInfoControl)d;
            sender.Symbol_Text.Text = (string)e.NewValue;
        }

        #endregion

        #region Prozent

        public double Prozent
        {
            get { return (double)GetValue(ProzentProperty); }
            set { SetValue(ProzentProperty, value); }
        }

        public static readonly DependencyProperty ProzentProperty =
        DependencyProperty.Register("Prozent", typeof(double), typeof(AktieQuickInfoControl), new PropertyMetadata(ProzentChangedCallback));

        private static void ProzentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieQuickInfoControl sender = (AktieQuickInfoControl)d;
            sender.Prozent_Text.Text = string.Format("{0:F2}%", (double)e.NewValue);
        }

        #endregion

        #region NominalWert

        public double NominalWert
        {
            get { return (double)GetValue(NominalProperty); }
            set { SetValue(NominalProperty, value); }
        }

        public static readonly DependencyProperty NominalProperty =
        DependencyProperty.Register("NominalWert", typeof(double), typeof(AktieQuickInfoControl), new PropertyMetadata(NominalChangedCallback));

        private static void NominalChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AktieQuickInfoControl sender = (AktieQuickInfoControl)d;
            sender.Nominal_Text.Text = string.Format("{0:F2}", (double)e.NewValue);
        }

        #endregion

        public AktieQuickInfoControl()
        {
            InitializeComponent();
        }
    }
}