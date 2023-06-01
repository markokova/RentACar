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

namespace RentACar.WebApi.Controllers
{
    public class PersonController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetPersons()
        {
            PersonService personService = new PersonService();
            List<Person> people =  personService.GetPeople();
            List<PersonRest> peopleRest = new List<PersonRest>();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            peopleRest = personMapper.MapToRest(people);

            if (peopleRest.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "There is no Persons in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, peopleRest);
        }

        [HttpGet]
        public HttpResponseMessage GetPerson(Guid id)
        {
            PersonService personService = new PersonService();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();


            List<Person> people = new List<Person>();

            people.Add(personService.GetPerson(id));

            List<PersonRest> peopleRest = personMapper.MapToRest(people);

            if (people.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no Person with Id:{id} in the database.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, peopleRest);
        }

        [HttpPost]
        public HttpResponseMessage SaveNewPerson([FromBody] PersonRest personRest)
        {
            PersonService personService = new PersonService();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            Person person = personMapper.MapRestToDomain(personRest);

            int affectedRows = personService.SavePerson(person);

            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, affectedRows);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Object isn't inserted.");
        }

        [HttpPut]
        public HttpResponseMessage UpdatePerson(Guid id, [FromBody] PersonRest personRest)
        {
            PersonService personService = new PersonService();
            RestDomainPersonMapper personMapper = new RestDomainPersonMapper();

            Person person = personMapper.MapRestToDomain(personRest);

            int affectedRows = personService.UpdatePerson(id, person);
            if (affectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, id);
        }

        [HttpDelete]
        public HttpResponseMessage DeletePerson(Guid id)
        {
            PersonService personService = new PersonService();
            int affectedRows = personService.DeletePerson(id);

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