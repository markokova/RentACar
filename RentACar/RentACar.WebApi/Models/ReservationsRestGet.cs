using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Models
{
    public class ReservationsRestGet
    {
        public List<ReservationRestGet> Reservations { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}