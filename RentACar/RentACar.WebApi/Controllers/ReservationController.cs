using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using RentACar.Model;
using RentACar.WebApi.Models;
using RentACar.Service;
using RentACar.Common.Responses;
using RentACar.WebApi.Mappers;
using System.Threading.Tasks;
using RentACar.Common;
using System.Web.WebPages;
using System.Security.Policy;
using RentACar.Service.Common;

namespace RentAReservation.WebApi.Controllers
{
    public class ReservationController : ApiController
    {
        public IReservationService Service { get; set; }
        public ReservationController(IReservationService service)
        {
            Service = service;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetReservationsAsync(DateTime? fromDate = null, DateTime? toDate = null, int perPageNumber = 10, int currentPageNuber = 1, string orederBy = "Id", string sortOrder = "DESC")
        {
            Sorting sorting = new Sorting(); Paging paging = new Paging(); ReservationFiltering filtering = new ReservationFiltering();
            sorting.SortOrder = sortOrder; sorting.Orderby = orederBy; 
            paging.CurrentPageNumber = currentPageNuber; paging.PageSize = perPageNumber;
            
            if(fromDate == null){ filtering.FromDate = DateTime.MinValue; }
            else { filtering.FromDate = fromDate; }
            if(toDate == null) { filtering.ToDate = DateTime.MaxValue; }
            else { filtering.ToDate = toDate; }

            PagedList<Reservation> response = await Service.GetReservationsAsync(sorting, paging, filtering);
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            ReservationsRestGet reservationsRest = reservationMapper.MapToRest(response);

            if (reservationsRest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no Reservations in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { reservationsRest,  });
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveNewReservationAsync([FromBody] ReservationRestPost reservationRest)
        {
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            Reservation reservation = reservationMapper.MapRestToDomain(reservationRest);

            int affectedRows = await Service.SaveReservationAsync(reservation);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, reservation);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateReservationAsync(Guid id, [FromBody] ReservationRestPost reservationRest)
        {
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            Reservation reservation = reservationMapper.MapRestToDomain(reservationRest);

            int affectedRows = await Service.UpdateReservationAsync(id, reservation);
            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteReservationAsync(Guid id)
        {
            int affectedRows = await Service.DeleteReservationAsync(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Reservation with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }
    }
}