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

namespace RentAReservation.WebApi.Controllers
{
    public class ReservationController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetReservations()
        {
            ReservationService reservationService = new ReservationService();
            List<ReservationResponse> responses = reservationService.GetReservations();
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            List<ReservationRestGet> reservationsRest = reservationMapper.MapToRest(responses);

            if (reservationsRest.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no Reservations in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, reservationsRest);
        }
        [HttpGet]
        public HttpResponseMessage GetReservation(Guid id)
        {
            ReservationService reservationService = new ReservationService();

            ReservationResponse response = reservationService.GetReservation(id);

            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            ReservationRestGet reservationRest = reservationMapper.MapToRest(response);

            if (reservationRest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no Reservation with Id:{id} in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, reservationRest);
        }

        [HttpPost]
        public HttpResponseMessage SaveNewReservation([FromBody] ReservationRestPost reservationRest)
        {
            ReservationService reservationService = new ReservationService();
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            Reservation reservation = reservationMapper.MapRestToDomain(reservationRest);

            int affectedRows = reservationService.SaveReservation(reservation);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, reservation);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public HttpResponseMessage UpdateReservation(Guid id, [FromBody] ReservationRestPost reservationRest)
        {
            ReservationService reservationService = new ReservationService();
            RestDomainReservationMapper reservationMapper = new RestDomainReservationMapper();

            Reservation reservation = reservationMapper.MapRestToDomain(reservationRest);

            int affectedRows = reservationService.UpdateReservation(id, reservation);
            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteReservation(Guid id)
        {
            ReservationService reservationService = new ReservationService();
            int affectedRows = reservationService.DeleteReservation(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Reservation with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }
    }
}