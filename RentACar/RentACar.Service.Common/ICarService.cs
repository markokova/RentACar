using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Model;

namespace RentACar.Service.Common
{
    public interface ICarService
    {
        Task<Car> GetCarAsync(Guid id);

        Task<List<Car>> GetCarsAsync();

        Task<int> SaveCarAsync(Car car);

        Task<int> UpdateCarAsync(Guid id, Car car);

        Task<int> DeleteCarAsync(Guid id);
    }
}
