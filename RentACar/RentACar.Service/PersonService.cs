using RentACar.Model;
using RentACar.Repository;
using RentACar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service
{
    public class PersonService : IPersonService
    {
        public int SavePerson(Person person)
        {
            PersonRepository personRepository = new PersonRepository();
            Guid id = Guid.NewGuid();
            person.Id = id;
            int affectedRows = personRepository.SavePerson(person);

            return affectedRows;
        }

        public List<Person> GetPeople()
        {
            PersonRepository personRepository = new PersonRepository();

            List<Person> Persons = personRepository.GetPeople();

            return Persons;
        }

        public Person GetPerson(Guid id)
        {
            PersonRepository personRepository = new PersonRepository();

            return personRepository.GetPerson(id);
        }

        public int UpdatePerson(Guid id, Person newPerson)
        {
            PersonRepository personRepository = new PersonRepository();

            return personRepository.UpdatePerson(id, newPerson);
        }

        public int DeletePerson(Guid id)
        {
            PersonRepository personRepository = new PersonRepository();

            return personRepository.DeletePerson(id);
        }
    }
}
