using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using RentACar.Service;
using RentACar.Model;
using RentACar.WebApi.Models;
using RentACar.WebApi.Mappers;
using System.Threading.Tasks;

namespace RentACar.WebApi.Controllers
{
    public class CarController : ApiController
    {

        [HttpGet]
        public async Task<HttpResponseMessage> GetCarsAsync()
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            List<Car> cars = await carService.GetCarsAsync();
            List<CarRest> carsRest = new List<CarRest>();

            carsRest = carMapper.MapToRest(cars);

            if (carsRest.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no cars in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, carsRest);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetCarAsync(Guid id)
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            List<Car> cars = new List<Car>();

            cars.Add(await carService.GetCarAsync(id));


            List<CarRest> carsRest = carMapper.MapToRest(cars);

            if (cars.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no car with Id:{id} in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, carsRest);
        }

        //TODO - vidit sta cu s getcarbyprice

        //[HttpGet]
        //public HttpResponseMessage GetCarByPrice(double price)
        //{
        //    CarService carService = new CarService();

        //    List<Car> cars = carService.GetCarByPrice(price);

        //    if (cars.Count == 0)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "There are no cars this cheap.");
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, cars);
        //}

        [HttpPost]
        public async Task<HttpResponseMessage> SaveNewCarAsync([FromBody] CarRest carRest)
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            Car car = carMapper.MapRestToDomain(carRest);

            int affectedRows = await carService.SaveCarAsync(car);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, car);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateCarAsync(Guid id, [FromBody] CarRest carRest)
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            Car car = carMapper.MapRestToDomain(carRest);

            int affectedRows = await carService.UpdateCarAsync(id, car);
            if (affectedRows > 0)
            {

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteCarAsync(Guid id)
        {
            CarService carService = new CarService();
            int affectedRows = await carService.DeleteCarAsync(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Car with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }
    }
}