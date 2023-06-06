using RentACar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Common.Responses;
using RentACar.Common;

namespace RentACar.Repository.Common
{
    public interface IReservationRepository
    {
        Task<int> SaveReservationAsync(Reservation reservation);

        Task<PagedList<Reservation>> GetReservationsAsync(Sorting sorting, Paging paging, ReservationFiltering filtering);

        //Task<ReservationResponse> GetReservationAsync(Guid id);

        Task<int> UpdateReservationAsync(Guid id, Reservation reservation);

        Task<int> DeleteReservationAsync(Guid id);
    }
}
