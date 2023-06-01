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
        int SaveCar(Car car);

        List<Car> GetCars();

        Car GetCar(Guid id);

        int UpdateCar(Guid id, Car car);

        int DeleteCar(Guid id);
    }
}
