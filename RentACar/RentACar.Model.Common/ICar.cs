using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Model.Common
{
    public class ICar
    {
         Guid Id { get; set; }

         string Manufacturer { get; set; }

         string Model { get; set; }

         int NumberOfSeats { get; set; }

         double Price { get; set; }
    }
}
