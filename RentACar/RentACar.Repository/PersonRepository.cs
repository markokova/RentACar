using Npgsql;
using RentACar.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository
{
    public class PersonRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public int InsertPerson(Person Person)
        {
            Guid id = Guid.NewGuid();
            Person.Id = id;
            int affectedRows = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO \"Person\" (\"Id\", \"FirstName\", \"LastName\", \"Email\") VALUES (@Value1, @Value2, @Value3, @Value4)";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", Person.Id);
                        command.Parameters.AddWithValue("@Value2", Person.FirstName);
                        command.Parameters.AddWithValue("@Value3", Person.LastName);
                        command.Parameters.AddWithValue("@Value4", Person.Email);
                        affectedRows = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return affectedRows;
        }

        public List<Person> GetPersons()
        {
            List<Person> Persons = new List<Person>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"Person\"";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Person Person = new Person();
                                Person.Id = (Guid)reader["Id"];
                                Person.FirstName = (string)reader["FirstName"];
                                Person.LastName = (string)reader["LastName"];
                                Person.Email = (string)reader["Email"];
                                Persons.Add(Person);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return Persons;
        }

        public Person GetPerson(Guid id)
        {
            return this.GetPersonById(id);
        }

        public int UpdatePerson(Guid id, Person newPerson)
        {
            Person oldPerson = GetPersonById(id);
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
                            command.Parameters.AddWithValue("@OldIdValue", oldPerson.Id);
                            command.CommandText = query;
                            command.Connection = connection;
                            affectedRows = command.ExecuteNonQuery();
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

        public int DeletePerson(Guid id)
        {
            Person PersonToDelete = this.GetPersonById(id);
            int affectedRows = 0;
            try
            {
                if (PersonToDelete != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM \"Person\" WHERE \"Id\" = @IdValue";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdValue", id);
                            affectedRows = command.ExecuteNonQuery();
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

        private Person GetPersonById(Guid id)
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
                        NpgsqlDataReader reader = command.ExecuteReader();
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
