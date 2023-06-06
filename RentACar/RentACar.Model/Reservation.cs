using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Model.Common;

namespace RentACar.Model
{
    public class Reservation : IReservation
    {
        public Guid Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public Guid CarId { get; set; }

        public Guid PersonId { get; set; }

        public Car Car { get; set; }

        public Person Person { get; set; }
    }
}
