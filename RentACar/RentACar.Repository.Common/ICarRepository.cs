using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Model;

namespace RentACar.Repository.Common
{
    public interface ICarRepository
    {
        Task<int> SaveCarAsync(Car car);

        Task<List<Car>> GetCarsAsync();

        Task<Car> GetCarAsync(Guid id);

        Task<int> UpdateCarAsync(Guid id, Car car);

        Task<int> DeleteCarAsync(Guid id);
    }
}
