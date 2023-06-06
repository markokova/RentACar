using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Models
{
    public class CarsRest
    {
        public List<CarRest> CarsRestList { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}