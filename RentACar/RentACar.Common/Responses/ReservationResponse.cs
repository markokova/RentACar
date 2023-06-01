using RentACar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Common.Responses
{
    public class ReservationResponse
    {
        public Reservation reservation { get; set; }

        public Car car { get; set; }

        public Person person { get; set; }
    }
}
