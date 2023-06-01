using RentACar.Common.Responses;
using RentACar.Model;
using RentACar.Repository;
using RentACar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service
{
    public class ReservationService : IReservationService
    {
        public int SaveReservation(Reservation reservation)
        {
            ReservationRepository reservationRepository = new ReservationRepository();
            Guid id = Guid.NewGuid();
            reservation.Id = id;
            int affectedRows = reservationRepository.SaveReservation(reservation);
            return affectedRows;
        }

        public List<ReservationResponse> GetReservations()
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return reservationRepository.GetReservations();
        }

        public ReservationResponse GetReservation(Guid id)
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return reservationRepository.GetReservation(id);
        }

        public int UpdateReservation(Guid id, Reservation newReservation)
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return reservationRepository.UpdateReservation(id, newReservation);
        }

        public int DeleteReservation(Guid id)
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return reservationRepository.DeleteReservation(id);
        }
    }
}
