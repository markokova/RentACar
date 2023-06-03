using RentACar.Common;
using RentACar.Common.Responses;
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
        public async Task<int> SaveCarAsync(Car car)
        {
            CarRepository carRepository = new CarRepository();
            Guid id = Guid.NewGuid();
            car.Id = id;
            return await carRepository.SaveCarAsync(car);
        }

        public async Task<CarsResponse> GetCarsAsync(Paging paging, Sorting sorting, CarFiltering filtering)
        {
            CarRepository carRepository = new CarRepository();

            return await carRepository.GetCarsAsync(paging, sorting, filtering);
        }

        public async Task<Car> GetCarAsync(Guid id)
        {
            CarRepository carRepository = new CarRepository();

            return await carRepository.GetCarAsync(id);
        }

        public async Task<List<Car>> GetCarByPriceAsync(double price)
        {
            CarRepository carRepository = new CarRepository();

            return await carRepository.GetCarByPriceAsync(price);
        }

        public async Task<int> UpdateCarAsync(Guid id, Car newCar)
        {
            CarRepository carRepository = new CarRepository();
            return await carRepository.UpdateCarAsync(id, newCar);
        }

        public async Task<int> DeleteCarAsync(Guid id)
        {
            CarRepository carRepository = new CarRepository();

            return await carRepository.DeleteCarAsync(id);
        }
    }
}
