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

namespace RentAReservation.WebApi.Controllers
{
    public class ReservationController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> GetReservationsAsync()
        {
            ReservationService reservationService = new ReservationService();
            List<ReservationResponse> responses = await reservationService.GetReservationsAsync();
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            List<ReservationRestGet> reservationsRest = reservationMapper.MapToRest(responses);

            if (reservationsRest.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no Reservations in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, reservationsRest);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetReservationAsync(Guid id)
        {
            ReservationService reservationService = new ReservationService();

            ReservationResponse response = await reservationService.GetReservationAsync(id);

            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            ReservationRestGet reservationRest = reservationMapper.MapToRest(response);

            if (reservationRest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no Reservation with Id:{id} in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, reservationRest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveNewReservationAsync([FromBody] ReservationRestPost reservationRest)
        {
            ReservationService reservationService = new ReservationService();
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            Reservation reservation = reservationMapper.MapRestToDomain(reservationRest);

            int affectedRows = await reservationService.SaveReservationAsync(reservation);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, reservation);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateReservationAsync(Guid id, [FromBody] ReservationRestPost reservationRest)
        {
            ReservationService reservationService = new ReservationService();
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            Reservation reservation = reservationMapper.MapRestToDomain(reservationRest);

            int affectedRows = await reservationService.UpdateReservationAsync(id, reservation);
            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteReservationAsync(Guid id)
        {
            ReservationService reservationService = new ReservationService();
            int affectedRows = await reservationService.DeleteReservationAsync(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Reservation with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }
    }
}