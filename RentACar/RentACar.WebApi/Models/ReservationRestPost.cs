using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Models
{
    public class ReservationRestPost
    {
        public DateTime ReservationDate { get; set; }

        public Guid PersonId { get; set; }

        public Guid CarId { get; set; }
    }
}