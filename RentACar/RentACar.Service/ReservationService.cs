using RentACar.Common;
using RentACar.Common.Responses;
using RentACar.Model;
using RentACar.Repository;
using RentACar.Repository.Common;
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
        public IReservationRepository Repository { get; set; }
        public ReservationService(IReservationRepository repository)
        {
            Repository = repository;   
        }
        public async Task<int> SaveReservationAsync(Reservation reservation)
        {
            Guid id = Guid.NewGuid();
            reservation.Id = id;
            int affectedRows = await Repository.SaveReservationAsync(reservation);
            return affectedRows;
        }

        public async Task<PagedList<Reservation>> GetReservationsAsync(Sorting sorting, Paging paging, ReservationFiltering filtering)
        {
            return await Repository.GetReservationsAsync(sorting, paging, filtering);
        }

        public async Task<int> UpdateReservationAsync(Guid id, Reservation newReservation)
        {
            return await Repository.UpdateReservationAsync(id, newReservation);
        }

        public async Task<int> DeleteReservationAsync(Guid id)
        {
            return await Repository.DeleteReservationAsync(id);
        }
    }
}
