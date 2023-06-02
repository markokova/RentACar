using Npgsql;
using RentACar.Model;
using RentACar.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public async Task<int> SavePersonAsync(Person person)
        {
            int affectedRows = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO \"Person\" (\"Id\", \"FirstName\", \"LastName\", \"Email\") VALUES (@Value1, @Value2, @Value3, @Value4)";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", person.Id);
                        command.Parameters.AddWithValue("@Value2", person.FirstName);
                        command.Parameters.AddWithValue("@Value3", person.LastName);
                        command.Parameters.AddWithValue("@Value4", person.Email);
                        affectedRows =  await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return affectedRows;
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            List<Person> people = new List<Person>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"Person\"";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Person person = new Person();
                                person.Id = (Guid)reader["Id"];
                                person.FirstName = (string)reader["FirstName"];
                                person.LastName = (string)reader["LastName"];
                                person.Email = (string)reader["Email"];
                                people.Add(person);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return people;
        }

        public async Task<Person> GetPersonAsync(Guid id)
        {
            return await this.GetPersonByIdAsync(id);
        }

        public async Task<int> UpdatePersonAsync(Guid id, Person newPerson)
        {
            Person oldPerson = await GetPersonByIdAsync(id);
            int affectedRows = 0;
            StringBuilder builder = new StringBuilder("UPDATE \"Person\" SET ");

            try
            {
                if (oldPerson != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            if (!string.IsNullOrEmpty(newPerson.FirstName))
                            {
                                builder.Append("\"FirstName\" = @FirstNameValue,");
                                command.Parameters.AddWithValue("@FirstNameValue", newPerson.FirstName);

                            }
                            if (!string.IsNullOrEmpty(newPerson.LastName))
                            {
                                builder.Append("\"LastName\" = @LastNameValue,");
                                command.Parameters.AddWithValue("@LastNameValue", newPerson.LastName);

                            }
                            if (!string.IsNullOrEmpty(newPerson.Email))
                            {
                                builder.Append("\"Email\" = @EmailValue,");
                                command.Parameters.AddWithValue("@EmailValue", newPerson.Email);
                            }
                            if (builder.ToString().EndsWith(","))
                            {
                                if (builder.Length > 0)
                                {
                                    builder.Remove(builder.Length - 1, 1);
                                }
                            }
                            builder.Append(" WHERE \"Id\" = @OldIdValue");
                            string query = builder.ToString();
                            command.CommandText = query;
                            command.Connection = connection;
                            command.Parameters.AddWithValue("@OldIdValue", oldPerson.Id);
                            affectedRows = await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return affectedRows;
        }

        public async Task<int> DeletePersonAsync(Guid id)
        {
            Person PersonToDelete = await this.GetPersonByIdAsync(id);
            int affectedRows = 0;
            try
            {
                if (PersonToDelete != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM \"Reservation\" WHERE \"PersonId\" = @PersonIdValue";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@PersonIdValue", id);
                            affectedRows = await command.ExecuteNonQueryAsync();
                        }
                        string query2 = "DELETE FROM \"Person\" WHERE \"Id\" = @IdValue";
                        using (NpgsqlCommand command = new NpgsqlCommand(query2, connection))
                        {
                            command.Parameters.AddWithValue("@IdValue", id);
                            affectedRows = await command.ExecuteNonQueryAsync();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return affectedRows;
        }

        private async Task<Person> GetPersonByIdAsync(Guid id)
        {
            Person Person = null;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"Person\" WHERE \"Id\" = @Value";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value", id);
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            Person = new Person();
                            reader.Read();
                            Person.Id = (Guid)reader["Id"];
                            Person.FirstName = (string)reader["FirstName"];
                            Person.LastName = (string)reader["LastName"];
                            Person.Email = (string)reader["Email"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return Person;
        }
    }
}
