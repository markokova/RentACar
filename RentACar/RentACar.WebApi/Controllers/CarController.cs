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

namespace RentACar.WebApi.Controllers
{
    public class CarController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetCars()
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            List<Car> cars = carService.GetCars();
            List<CarRest> carsRest = new List<CarRest>();

            carsRest = carMapper.MapToRest(cars);

            if (carsRest.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no cars in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, carsRest);
        }

        [HttpGet]
        public HttpResponseMessage GetCar(Guid id)
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            List<Car> cars = new List<Car>();

            cars.Add(carService.GetCar(id));


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
        public HttpResponseMessage SaveNewCar([FromBody] CarRest carRest)
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            Car car = carMapper.MapRestToDomain(carRest);

            int affectedRows = carService.SaveCar(car);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, car);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public HttpResponseMessage UpdateCar(Guid id, [FromBody] CarRest carRest)
        {
            CarService carService = new CarService();
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            Car car = carMapper.MapRestToDomain(carRest);

            int affectedRows = carService.UpdateCar(id, car);
            if (affectedRows > 0)
            {

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteCar(Guid id)
        {
            CarService carService = new CarService();
            int affectedRows = carService.DeleteCar(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Car with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        //private List<CarRest> MapToRest(List<Car> cars)
        //{
        //    List<CarRest> carsRest = new List<CarRest>();

        //    foreach (Car car in cars)
        //    {
        //        CarRest carRest = new CarRest();
        //        carRest.Manufacturer = car.Manufacturer;
        //        carRest.Model = car.Model;
        //        carRest.NumberOfSeats = car.NumberOfSeats;
        //        carRest.Price = car.Price;
        //        carsRest.Add(carRest);
        //    }
        //    return carsRest;
        //}

        //private Car MapRestToDomain(CarRest carRest)
        //{
        //    Car car = new Car();
        //    car.Manufacturer = carRest.Manufacturer;
        //    car.Model = carRest.Model;
        //    car.NumberOfSeats = carRest.NumberOfSeats;
        //    car.Price = carRest.Price;

        //    return car;
        //}
    }
}