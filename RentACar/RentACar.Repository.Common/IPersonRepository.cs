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
        Task<int> SavePersonAsync(Person person);

        Task<List<Person>> GetPeopleAsync();

        Task<Person> GetPersonAsync(Guid id);

        Task<int> UpdatePersonAsync(Guid id, Person person);

        Task<int> DeletePersonAsync(Guid id);
    }
}
