using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Model.Common
{
    public class IReservation
    {
         Guid Id { get; set; }

         DateTime ReservationDate { get; set; }

         Guid CarId { get; set; }

         Guid PersonId { get; set; }

         ICar Car { get; set; }

         IPerson Person { get; set; }
    }
}
