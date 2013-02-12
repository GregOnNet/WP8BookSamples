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
using System.ComponentModel;

namespace Aktienkurs.Controls.Sample
{
    public class TestVM : INotifyPropertyChanged
    {
        public TestVM()
        {
            Titel = "Hallo";
            Zahl = 42;
        }

        private string _titel;

        public string Titel
        {
            get { return _titel; }
            set
            {
                _titel = value;
                SendPropertyChanged("Titel");
            }
        }

        private double _zahl;

        public double Zahl
        {
            get { return _zahl; }
            set
            {
                _zahl = value;
                SendPropertyChanged("Zahl");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SendPropertyChanged(string propName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
