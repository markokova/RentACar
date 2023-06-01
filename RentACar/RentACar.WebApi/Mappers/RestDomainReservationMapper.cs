using RentACar.Common.Responses;
using RentACar.Model;
using RentACar.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Mappers
{
    public class RestDomainReservationMapper
    {
        public List<ReservationRestGet> MapToRest(List<ReservationResponse> reservationResponses)
        {
            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            List<ReservationRestGet> reservationsRest = new List<ReservationRestGet>();

            foreach (ReservationResponse reservationResponse in reservationResponses)
            {
                ReservationRestGet reservationRest = new ReservationRestGet();

                reservationRest.ReservationDate = reservationResponse.reservation.ReservationDate;
                reservationRest.PersonRest = personMapper.MapToRest(reservationResponse.person);
                reservationRest.CarRest = carMapper.MapToRest(reservationResponse.car);
                reservationsRest.Add(reservationRest);
            }
            return reservationsRest;
        }

        public ReservationRestGet MapToRest(ReservationResponse reservationResponse)
        {
            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            ReservationRestGet reservationRest = new ReservationRestGet();

            reservationRest.ReservationDate = reservationResponse.reservation.ReservationDate;
            reservationRest.PersonRest = personMapper.MapToRest(reservationResponse.person);
            reservationRest.CarRest = carMapper.MapToRest(reservationResponse.car);

            if(reservationResponse.reservation.Id == Guid.Empty)
            {
                return null;
            }
            return reservationRest;
        }

        public Reservation MapRestToDomain(ReservationRestPost reservationRest)
        {
            Reservation reservation = new Reservation();
            reservation.ReservationDate = reservationRest.ReservationDate;
            reservation.PersonId = reservationRest.PersonId;
            reservation.CarId = reservationRest.CarId;
            return reservation;
        }
    }
}