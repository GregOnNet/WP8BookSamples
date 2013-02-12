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

namespace MVVMSample.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int YearOfManufacture { get; set; }
        public int Ps { get; set; }
        public int MaxKmH { get; set; }

        public string Producer { get; set; }
        public string Name { get; set; }

        public bool HasDetails { get; set; }
    }
}
