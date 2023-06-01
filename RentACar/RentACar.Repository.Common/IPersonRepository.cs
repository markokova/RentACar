using RentACar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.Common
{
    public interface IPersonRepository
    {
        int SavePerson(Person person);

        List<Person> GetPeople();

        Person GetPerson(Guid id);

        int UpdatePerson(Guid id, Person person);

        int DeletePerson(Guid id);
    }
}
