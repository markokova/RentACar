using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Models
{
    public class CarRest
    {
        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public int NumberOfSeats { get; set; }

        public double Price { get; set; }
    }
}