using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Common;
using RentACar.Common.Responses;
using RentACar.Model;

namespace RentACar.Service.Common
{
    public interface ICarService
    {
        Task<Car> GetCarAsync(Guid id);

        Task<CarsResponse> GetCarsAsync(Paging paging, Sorting sorting, CarFiltering filtering);

        Task<int> SaveCarAsync(Car car);

        Task<int> UpdateCarAsync(Guid id, Car car);

        Task<int> DeleteCarAsync(Guid id);
    }
}
