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
        Car GetCar(Guid id);

        List<Car> GetCars();

        int SaveCar(Car car);

        int UpdateCar(Guid id, Car car);

        int DeleteCar(Guid id);
    }
}
