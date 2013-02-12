using System.Collections.Generic;
using System.Collections.ObjectModel;
using MVVMSample.Models;
using MVVMSample.Services;
using System.Windows.Controls;
using System;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using Phone.Helpers.Services;

namespace MVVMSample.ViewModels
{
    public class CarForListCollectionViewModel:
        ObservableCollection<CarForListViewModel>
    {
        public RelayCommand<CarForListViewModel> 
            NavigateToDetailsCommand { get; private set; }

        public CarForListCollectionViewModel()
        {
            NavigateToDetailsCommand = 
                new RelayCommand<CarForListViewModel>
                    (NavigateToDetails,
                        CanNavigateToDetails);

            LoadCars();
        }

        public void NavigateToDetails(CarForListViewModel car)
        {
            if (car != null)
            {
                var navigationUri = 
                    new Uri("/Views/CarDetailsView.xaml?id=" 
                           + car.Id,
                           UriKind.Relative);

                AppNavigationService
                    .NavigateTo(navigationUri);
            }
        }

        public bool CanNavigateToDetails(CarForListViewModel car)
        {
            if (car == null)
                return false;

            return car.HasDetails;
        }
        
        private void LoadCars()
        {

            foreach (Car car in CarSampleDataService.Cars)
            {
                this.Add(
                    new CarForListViewModel
                    {
                        Id = car.Id,
                        Producer = car.Producer,
                        Name = car.Name,
                        YearOfManufacture = car.YearOfManufacture,
                        HasDetails = car.HasDetails
                    });
            }
        }


    }
}
