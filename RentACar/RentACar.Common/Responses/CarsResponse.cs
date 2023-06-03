using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Model;

namespace RentACar.Common.Responses
{
    public class CarsResponse
    {
        public List<Car> Cars { get; set; }

        public int TotalNumberOfResults { get; set; }
    }
}
