using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Models
{
    public class ReservationRestGet
    {
        public DateTime ReservationDate { get; set; }

        public PersonRest PersonRest { get; set; }

        public CarRest CarRest { get; set; }

    }
}