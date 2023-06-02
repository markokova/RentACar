using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using RentACar.Service;
using RentACar.Model;
using RentACar.WebApi.Models;
using RentACar.WebApi.Mappers;
using System.Threading.Tasks;

namespace RentACar.WebApi.Controllers
{
    public class PersonController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> GetPeopleAsync()
        {
            PersonService personService = new PersonService();
            List<Person> people =  await personService.GetPeopleAsync();
            
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            List<PersonRest> peopleRest = personMapper.MapToRest(people);

            if (peopleRest.Count == 0 || peopleRest[0] == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There are no people in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, peopleRest);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetPersonAsync(Guid id)
        {
            PersonService personService = new PersonService();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();


            List<Person> people = new List<Person>();

            people.Add(await personService.GetPersonAsync(id));

            List<PersonRest> peopleRest = personMapper.MapToRest(people);

            if (people.Count == 0 || people[0] == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no Person with Id:{id} in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, peopleRest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveNewPersonAsync([FromBody] PersonRest personRest)
        {
            PersonService personService = new PersonService();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            Person person = personMapper.MapRestToDomain(personRest);

            int affectedRows = await personService.SavePersonAsync(person);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, affectedRows);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdatePersonAsync(Guid id, [FromBody] PersonRest personRest)
        {
            PersonService personService = new PersonService();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            Person person = personMapper.MapRestToDomain(personRest);

            int affectedRows = await personService.UpdatePersonAsync(id, person);
            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeletePersonAsync(Guid id)
        {
            PersonService personService = new PersonService();
            int affectedRows = await personService.DeletePersonAsync(id);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Person with Id: {id} deleted.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        //private List<PersonRest> MapToRest(List<Person> people)
        //{
        //    List<PersonRest> peopleRest = new List<PersonRest>();
            
        //    foreach(Person person in people)
        //    {
        //        PersonRest personRest = new PersonRest();
        //        personRest.FirstName = person.FirstName;
        //        personRest.LastName = person.LastName;
        //        personRest.Email = person.Email;
        //        peopleRest.Add(personRest);
        //    }
        //    return peopleRest;
        //}

        //private Person MapRestToDomain(PersonRest personRest)
        //{
        //    Person person = new Person();
        //    person.FirstName = personRest.FirstName;
        //    person.LastName = personRest.LastName;
        //    person.Email = personRest.Email;

        //    return person;
        //}
    }
}