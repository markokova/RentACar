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
        int SaveReservation(Reservation reservation);

        List<ReservationResponse> GetReservations();

        ReservationResponse GetReservation(Guid id);

        int UpdateReservation(Guid id, Reservation reservation);

        int DeleteReservation(Guid id);
    }
}
