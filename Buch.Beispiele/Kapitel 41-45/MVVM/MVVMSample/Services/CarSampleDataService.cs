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
using MVVMSample.Models;

namespace MVVMSample.Services
{
    public static class CarSampleDataService
    {
        public static IEnumerable<Car> Cars
        {
            get
            {
                return GetCars();
            }
        }

        public static IEnumerable<Car> GetCars()
        {
            return new Car[] {
                new Car
                {
                    Id = 1,
                    Producer = "VW",
                    Name="Golf 5 Variant",
                    YearOfManufacture = 2007,
                    MaxKmH = 205,
                    Ps = 140,
                    HasDetails = true
                },
                new Car
                {
                    Id = 2,
                    Producer = "Audi",
                    Name="S5 Coupe",
                    YearOfManufacture = 2007,
                    MaxKmH = 250,
                    Ps = 354,
                    HasDetails = true
                },
                new Car
                {
                    Id = 3,
                    Producer = "Skoda",
                    Name="Fabia Kombi",
                    YearOfManufacture = 2010,
                    MaxKmH = 167,
                    Ps = 75,
                    HasDetails = false
                },
                new Car
                {
                    Id = 4,
                    Producer = "VEB Sachsenring Automobilwerke Zwickau",
                    Name = "Trabant",
                    YearOfManufacture = 1973,
                    MaxKmH = 125,
                    Ps = 26,
                    HasDetails = false
                }
            };
        }
    }
}
