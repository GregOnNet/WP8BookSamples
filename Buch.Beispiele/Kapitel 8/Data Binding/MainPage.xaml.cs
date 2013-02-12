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

namespace DataBinding
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private fields
        private ViewModel _currentViewModel;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        
        //void MainPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //Daten initialisieren.
        //    _currentViewModel = new ViewModel();
        //    //Daten an die Oberfläche binden.
        //    this.DataContext = _currentViewModel;
        //}
    }
}
