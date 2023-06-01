using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using RentACar.Service;
using RentACar.Common.Responses;
using RentACar.Model;

namespace RentACar.WebApi.Controllers
{
    public class ReservationController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetReservations()
        {
            ReservationService reservationService = new ReservationService();
            List<ReservationResponse> responses = reservationService.GetReservations();
            if (responses.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no Reservations in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, responses);
        }
        [HttpGet]
        public HttpResponseMessage GetReservation(Guid id)
        {
            ReservationService reservationService = new ReservationService();

            ReservationResponse response = reservationService.GetReservation(id);

            if (response.reservation.Id == Guid.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no Reservation with Id:{id} in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage SaveNewReservation([FromBody] Reservation Reservation)
        {
            ReservationService reservationService = new ReservationService();
            int affectedRows = reservationService.SaveReservation(Reservation);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Reservation);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public HttpResponseMessage UpdateReservation(Guid id, [FromBody] Reservation Reservation)
        {
            ReservationService reservationService = new ReservationService();
            int affectedRows = reservationService.UpdateReservation(id, Reservation);
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