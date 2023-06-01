using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Model;

namespace RentACar.Service.Common
{
    public interface IPersonService
    {
        Person GetPerson(Guid id);

        List<Person> GetPeople();

        int SavePerson(Person person);

        int UpdatePerson(Guid id, Person person);

        int DeletePerson(Guid id);
    }
}
