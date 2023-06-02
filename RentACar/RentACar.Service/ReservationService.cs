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
        public async Task<int> SaveReservationAsync(Reservation reservation)
        {
            ReservationRepository reservationRepository = new ReservationRepository();
            Guid id = Guid.NewGuid();
            reservation.Id = id;
            int affectedRows = await reservationRepository.SaveReservationAsync(reservation);
            return affectedRows;
        }

        public async Task<List<ReservationResponse>> GetReservationsAsync()
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return await reservationRepository.GetReservationsAsync();
        }

        public async Task<ReservationResponse> GetReservationAsync(Guid id)
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return await reservationRepository.GetReservationAsync(id);
        }

        public async Task<int> UpdateReservationAsync(Guid id, Reservation newReservation)
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return await reservationRepository.UpdateReservationAsync(id, newReservation);
        }

        public async Task<int> DeleteReservationAsync(Guid id)
        {
            ReservationRepository reservationRepository = new ReservationRepository();

            return await reservationRepository.DeleteReservationAsync(id);
        }
    }
}
