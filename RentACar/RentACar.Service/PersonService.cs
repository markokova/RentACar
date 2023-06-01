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
        public int SavePerson(Person Person)
        {
            PersonRepository personRepository = new PersonRepository();
            //Guid id = Guid.NewGuid();
            //Person.Id = id;
            int affectedRows = personRepository.InsertPerson(Person);

            return affectedRows;
        }

        public List<Person> GetPeople()
        {
            PersonRepository personRepository = new PersonRepository();

            List<Person> Persons = personRepository.GetPersons();

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
