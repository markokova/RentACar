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
        public async Task<int> SavePersonAsync(Person person)
        {
            PersonRepository personRepository = new PersonRepository();
            Guid id = Guid.NewGuid();
            person.Id = id;
            int affectedRows = await personRepository.SavePersonAsync(person);

            return affectedRows;
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            PersonRepository personRepository = new PersonRepository();

            List<Person> Persons = await personRepository.GetPeopleAsync();

            return Persons;
        }

        public async Task<int> UpdatePersonAsync(Guid id, Person newPerson)
        {
            PersonRepository personRepository = new PersonRepository();

            return await personRepository.UpdatePersonAsync(id, newPerson);
        }

        public async Task<int> DeletePersonAsync(Guid id)
        {
            PersonRepository personRepository = new PersonRepository();

            return await personRepository.DeletePersonAsync(id);
        }
    }
}
