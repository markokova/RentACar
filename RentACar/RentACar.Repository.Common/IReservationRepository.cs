using RentACar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Common.Responses;

namespace RentACar.Repository.Common
{
    public interface IReservationRepository
    {
        Task<int> SaveReservationAsync(Reservation reservation);

        Task<List<ReservationResponse>> GetReservationsAsync();

        Task<ReservationResponse> GetReservationAsync(Guid id);

        Task<int> UpdateReservationAsync(Guid id, Reservation reservation);

        Task<int> DeleteReservationAsync(Guid id);
    }
}
