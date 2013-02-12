using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMSample.Services;
using Phone.Helpers.Services;

namespace MVVMSample.ViewModels
{
    public class CarDetailsViewModel
    {
        public int YearOfManufacture { get; set; }
        public int Ps { get; set; }
        public int MaxKmH { get; set; }

        public string Producer { get; set; }
        public string Name { get; set; }

        public RelayCommand SaveCommand { get; set; }

        public CarDetailsViewModel()
        {
            Messenger.Default.Register<int>
                                      (this,
                                      "LoadCarDetails",
                                      LoadCarById);
            SaveCommand = new RelayCommand(Save);
        }

        public void LoadCarById(int carId)
        {
            var car = CarSampleDataService
                        .Cars.FirstOrDefault(c => c.Id == carId);

          if (car == null)
            return;

          YearOfManufacture = car.YearOfManufacture;
          Ps = car.Ps;
          MaxKmH = car.MaxKmH;
          Producer = car.Producer;
          Name = car.Name;
        }

        public void Save()
        {
            //geänderte Daten speichern
            AppNavigationService.GoBack();
        }
    }
}
