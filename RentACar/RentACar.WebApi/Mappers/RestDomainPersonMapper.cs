using RentACar.Model;
using RentACar.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.WebApi.Mappers
{
    public class RestDomainPersonMapper
    {
        public List<PersonRest> MapToRest(List<Person> people)
        {
            List<PersonRest> peopleRest = new List<PersonRest>();

            foreach (Person person in people)
            {
                PersonRest personRest = new PersonRest();
                personRest.FirstName = person.FirstName;
                personRest.LastName = person.LastName;
                personRest.Email = person.Email;
                peopleRest.Add(personRest);
            }
            return peopleRest;
        }

        public PersonRest MapToRest(Person person)
        {
            PersonRest personRest = new PersonRest();
            personRest.FirstName = person.FirstName;
            personRest.LastName = person.LastName;
            personRest.Email = person.Email;

            return personRest;
        }

        public Person MapRestToDomain(PersonRest personRest)
        {
            Person person = new Person();
            person.FirstName = personRest.FirstName;
            person.LastName = personRest.LastName;
            person.Email = personRest.Email;

            return person;
        }
    }
}