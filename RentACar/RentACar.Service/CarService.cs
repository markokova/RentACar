using RentACar.Model;
using RentACar.Repository;
using RentACar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service
{
    public class CarService : ICarService
    {
        public int SaveCar(Car car)
        {
            CarRepository carRepository = new CarRepository();
            Guid id = Guid.NewGuid();
            car.Id = id;
            return carRepository.SaveCar(car);
        }

        public List<Car> GetCars()
        {
            CarRepository carRepository = new CarRepository();

            return carRepository.GetCars();
        }

        public Car GetCar(Guid id)
        {
            CarRepository carRepository = new CarRepository();

            return carRepository.GetCar(id);
        }

        public List<Car> GetCarByPrice(double price)
        {
            CarRepository carRepository = new CarRepository();

            return carRepository.GetCarByPrice(price);
        }

        public int UpdateCar(Guid id, Car newCar)
        {
            CarRepository carRepository = new CarRepository();
            return carRepository.UpdateCar(id, newCar);
        }

        public int DeleteCar(Guid id)
        {
            CarRepository carRepository = new CarRepository();

            return carRepository.DeleteCar(id);
        }
    }
}
