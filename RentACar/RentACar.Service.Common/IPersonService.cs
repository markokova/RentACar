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
        Task<Person> GetPersonAsync(Guid id);

        Task<List<Person>> GetPeopleAsync();

        Task<int> SavePersonAsync(Person person);

        Task<int> UpdatePersonAsync(Guid id, Person person);

        Task<int> DeletePersonAsync(Guid id);
    }
}
