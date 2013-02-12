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

namespace FirstUserControl.Controls
{
    public partial class LabeledTextBox : UserControl
    {
        public string Beschriftung
        {
            get { return BeschreibungsText.Text; }
            set { BeschreibungsText.Text = value; }
        }
        public string Text
        {
            get { return InhaltsText.Text; }
            set { InhaltsText.Text = value; }
        }

        public LabeledTextBox()
        {
            InitializeComponent();
        }
    }
}
