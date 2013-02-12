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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using GalaSoft.MvvmLight.Messaging;
using MVVMSample.ViewModels;

namespace MVVMSample.Views
{
    public partial class CarDetailsView : PhoneApplicationPage
    {
        public CarDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo
                                       (NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString
                                 .ContainsKey("id"))
            {
                string idParameter = 
                       NavigationContext.QueryString["id"];
                int id = 0;
                
                if (int.TryParse(idParameter, out id))
                {
                    Messenger.Default.Send<int>
                                          (id,
                                           "LoadCarDetails");
                }
            }
        }
    }
}