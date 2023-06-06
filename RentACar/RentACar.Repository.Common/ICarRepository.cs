using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Common;
using RentACar.Common.Responses;
using RentACar.Model;

namespace RentACar.Repository.Common
{
    public interface ICarRepository
    {
        Task<int> SaveCarAsync(Car car);

        Task<PagedList<Car>> GetCarsAsync(Paging paging, Sorting sorting, CarFiltering filtering);

        Task<int> UpdateCarAsync(Guid id, Car car);

        Task<int> DeleteCarAsync(Guid id);
    }
}
