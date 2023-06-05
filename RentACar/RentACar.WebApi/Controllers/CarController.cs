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
using RentACar.Common;
using RentACar.Common.Responses;
using RentACar.Service.Common;

namespace RentACar.WebApi.Controllers
{
    public class CarController : ApiController
    {
        protected ICarService Service { get; set; }

        public CarController(ICarService service)
        {
            Service = service;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetCarsAsync(double? minPrice = null, double? maxPrice = null, int? numberOfSeats = null, int pageSize = 10, int pageNumber = 1, string orderBy = "Id", string sortOrder = "DESC")
        {
            Sorting sorting = new Sorting(); Paging paging = new Paging(); CarFiltering filtering = new CarFiltering();
            paging.PageSize = pageSize; paging.CurrentPageNumber = pageNumber;
            sorting.Orderby = orderBy; sorting.SortOrder = sortOrder;
            filtering.MinPrice = minPrice; filtering.MaxPrice = maxPrice; filtering.NumberOfSeats = numberOfSeats;
            
            CarsResponse response = await Service.GetCarsAsync(paging, sorting, filtering);

            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            List<CarRest> carsRest =  carMapper.MapToRest(response.Cars);

            if (carsRest.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no cars in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { Cars = carsRest, TotalNumberOfResults = response.TotalNumberOfResults });
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetCarAsync(Guid id)
        {
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            List<Car> cars = new List<Car>();

            cars.Add(await Service.GetCarAsync(id));


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
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            Car car = carMapper.MapRestToDomain(carRest);

            int affectedRows = await Service.SaveCarAsync(car);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, car);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateCarAsync(Guid id, [FromBody] CarRest carRest)
        {
            RestDomainCarMapper carMapper = new RestDomainCarMapper();

            Car car = carMapper.MapRestToDomain(carRest);

            int affectedRows = await Service.UpdateCarAsync(id, car);
            if (affectedRows > 0)
            {

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteCarAsync(Guid id)
        {
            int affectedRows = await Service.DeleteCarAsync(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Car with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        //private Paging Paginate()
        //{
        //    Paging paging = new Paging();
        //}
    }
}