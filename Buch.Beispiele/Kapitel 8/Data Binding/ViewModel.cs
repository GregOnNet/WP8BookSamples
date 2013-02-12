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
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DataBinding
{
    public class ViewModel
    {
        public List<Model> PictureList { get; set; }
        
        public ViewModel()
        {
            PictureList = new List<Model>
            {
                new Model{EntryId=1,
                          Description = "We",
                          Image = new BitmapImage(new Uri("/Images/we.png",UriKind.Relative))},
                new Model{EntryId=2,
                          Description = "love",
                          Image = new BitmapImage(new Uri("/Images/love.png",UriKind.Relative))},
                new Model{EntryId=3,
                          Description = "Windows",
                          Image = new BitmapImage(new Uri("/Images/windows.png",UriKind.Relative))},
                new Model{EntryId=4,
                          Description = "Phone",
                          Image = new BitmapImage(new Uri("/Images/phone.png",UriKind.Relative))}
            };
        }
    }
}
