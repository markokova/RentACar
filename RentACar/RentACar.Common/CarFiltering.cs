using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Common
{
    public class CarFiltering
    {
        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public int? NumberOfSeats { get; set; }
    }
}
