using RentACar.Common;
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
        public CarsRest MapToRest(PagedList<Car> cars)
        {
            CarsRest carsRest = new CarsRest();
            carsRest.CarsRestList = new List<CarRest>();

            if(cars != null)
            {
                foreach (Car car in cars)
                {
                    CarRest carRest = new CarRest();
                    carRest.Manufacturer = car.Manufacturer;
                    carRest.Model = car.Model;
                    carRest.NumberOfSeats = car.NumberOfSeats;
                    carRest.Price = car.Price;
                    carsRest.CarsRestList.Add(carRest);
                }
            }
            carsRest.CurrentPage = cars.CurrentPage;
            carsRest.TotalPages = cars.TotalPages;
            carsRest.TotalCount = cars.TotalCount;
            carsRest.PageSize = cars.PageSize;
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