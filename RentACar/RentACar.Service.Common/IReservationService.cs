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
        ReservationResponse GetReservation(Guid id);

        List<ReservationResponse> GetReservations();

        int SaveReservation(Reservation reservation);

        int UpdateReservation(Guid id, Reservation reservation);

        int DeleteReservation(Guid id);
    }
}
