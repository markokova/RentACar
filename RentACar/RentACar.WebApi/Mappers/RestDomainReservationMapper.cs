using RentACar.Common;
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
        public ReservationsRestGet MapToRest(PagedList<Reservation> reservations)
        {
            //mappers
            RestDomainCarMapper carMapper = new RestDomainCarMapper();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            ReservationsRestGet reservationsRest = new ReservationsRestGet();
            reservationsRest.Reservations = new List<ReservationRestGet>();

            if (reservations != null)
            {
                foreach (Reservation reservation in reservations)
                {
                    ReservationRestGet reservationRestGet = new ReservationRestGet();

                    reservationRestGet.PersonRest = personMapper.MapToRest(reservation.Person);
                    reservationRestGet.CarRest = carMapper.MapToRest(reservation.Car);
                    reservationRestGet.ReservationDate = reservation.ReservationDate;

                    reservationsRest.Reservations.Add(reservationRestGet);
                }
            }
            reservationsRest.CurrentPage = reservations.CurrentPage;
            reservationsRest.TotalCount = reservations.Count;
            reservationsRest.PageSize = reservations.PageSize;
            reservationsRest.TotalPages = reservations.TotalPages;

            return reservationsRest;
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