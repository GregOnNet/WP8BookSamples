using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace MVVMSample.ViewModels
{
    public class CarForListViewModel
    {
        public string Producer { get; set; }

        public int Id { get; set; }
        public int YearOfManufacture { get; set; }

        public string Name { get; set; }

        public bool HasDetails { get; set; }

        public CarForListViewModel()
        { }
    }
}
