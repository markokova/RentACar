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
        protected  ICarService Service { get; set; }

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
            
            PagedList<Car> response = await Service.GetCarsAsync(paging, sorting, filtering);

            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            CarsRest carsRest =  carMapper.MapToRest(response);

            if (carsRest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no cars in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, carsRest);
        }


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
    }
}