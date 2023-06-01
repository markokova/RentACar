using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Model.Common
{
    public interface IPerson
    {
         Guid Id { get; set; }
         string FirstName { get; set; }

         string LastName { get; set; }

         string Email { get; set; }
    }
}
