using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Common.Responses;
using RentACar.Model;

namespace RentACar.Service.Common
{
    public interface IReservationService
    {
        Task<ReservationResponse> GetReservationAsync(Guid id);

        Task<List<ReservationResponse>> GetReservationsAsync();

        Task<int> SaveReservationAsync(Reservation reservation);

        Task<int> UpdateReservationAsync(Guid id, Reservation reservation);

        Task<int> DeleteReservationAsync(Guid id);
    }
}
