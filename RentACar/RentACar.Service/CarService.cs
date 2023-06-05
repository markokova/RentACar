using RentACar.Common;
using RentACar.Common.Responses;
using RentACar.Model;
using RentACar.Repository;
using RentACar.Repository.Common;
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
        protected ICarRepository Repository { get; set; }

        public CarService()
        {
            Repository = new CarRepository();
        }
        public async Task<int> SaveCarAsync(Car car)
        {
            Guid id = Guid.NewGuid();
            car.Id = id;
            return await Repository.SaveCarAsync(car);
        }

        public async Task<CarsResponse> GetCarsAsync(Paging paging, Sorting sorting, CarFiltering filtering)
        {

            return await Repository.GetCarsAsync(paging, sorting, filtering);
        }

        public async Task<Car> GetCarAsync(Guid id)
        {

            return await Repository.GetCarAsync(id);
        }


        public async Task<int> UpdateCarAsync(Guid id, Car newCar)
        {
            return await Repository.UpdateCarAsync(id, newCar);
        }

        public async Task<int> DeleteCarAsync(Guid id)
        {
            return await Repository.DeleteCarAsync(id);
        }
    }
}
