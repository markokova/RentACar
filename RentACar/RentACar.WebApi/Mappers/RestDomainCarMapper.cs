using RentACar.Model;
using RentACar.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Mappers
{
    public class RestDomainCarMapper
    {
        public List<CarRest> MapToRest(List<Car> cars)
        {
            List<CarRest> carsRest = new List<CarRest>();

            foreach (Car car in cars)
            {
                CarRest carRest = new CarRest();
                carRest.Manufacturer = car.Manufacturer;
                carRest.Model = car.Model;
                carRest.NumberOfSeats = car.NumberOfSeats;
                carRest.Price = car.Price;
                carsRest.Add(carRest);
            }
            return carsRest;
        }

        public CarRest MapToRest(Car car)
        {
            CarRest carRest = new CarRest();
            carRest.Manufacturer = car.Manufacturer;
            carRest.Model = car.Model;
            carRest.NumberOfSeats = car.NumberOfSeats;
            carRest.Price = car.Price;

            return carRest;
        }

        public Car MapRestToDomain(CarRest carRest)
        {
            Car car = new Car();
            car.Manufacturer = carRest.Manufacturer;
            car.Model = carRest.Model;
            car.NumberOfSeats = carRest.NumberOfSeats;
            car.Price = carRest.Price;

            return car;
        }
    }
}