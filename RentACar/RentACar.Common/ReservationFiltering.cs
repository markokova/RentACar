using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Common.Responses
{
    public class ReservationFiltering
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set;}
    }
}
