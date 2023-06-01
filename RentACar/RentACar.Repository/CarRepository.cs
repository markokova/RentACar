using Npgsql;
using RentACar.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using RentACar.Repository.Common;

namespace RentACar.Repository
{
    public class CarRepository : ICarRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public int SaveCar(Car car)
        {
            int affectedRows = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO \"Car\" (\"Id\", \"Manufacturer\", \"Model\", \"NumberOfSeats\", \"Price\") VALUES (@Value1, @Value2, @Value3, @Value4, @Value5)";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", car.Id);
                        command.Parameters.AddWithValue("@Value2", car.Manufacturer);
                        command.Parameters.AddWithValue("@Value3", car.Model);
                        command.Parameters.AddWithValue("@Value4", car.NumberOfSeats);
                        command.Parameters.AddWithValue("@Value5", car.Price);
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

        public List<Car> GetCars()
        {
            List<Car> cars = new List<Car>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"Car\"";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Car car = new Car();
                                car.Id = (Guid)reader["Id"];
                                car.Manufacturer = (string)reader["Manufacturer"];
                                car.Model = (string)reader["Model"];
                                car.NumberOfSeats = (int)reader["NumberOfSeats"];
                                car.Price = (double)reader["Price"];
                                cars.Add(car);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return cars;
        }

        public Car GetCar(Guid id)
        {
            return this.GetCarById(id);
        }

        public List<Car> GetCarByPrice(double price)
        {
            List<Car> cars = new List<Car>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "select * from \"Car\" where \"Car\".\"Price\" < @Price";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("Price", price);
                        NpgsqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Car car = new Car();
                                car.Id = (Guid)reader["Id"];
                                car.Manufacturer = (string)reader["Manufacturer"];
                                car.Model = (string)reader["Model"];
                                car.NumberOfSeats = (int)reader["NumberOfSeats"];
                                car.Price = (double)reader["Price"];
                                cars.Add(car);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return cars;
        }

        public int UpdateCar(Guid id, Car newCar)
        {
            Car oldCar = this.GetCarById(id);
            int affectedRows = 0;
            StringBuilder builder = new StringBuilder("UPDATE \"Car\" SET ");

            try
            {
                if (oldCar != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            if (!string.IsNullOrEmpty(newCar.Manufacturer))
                            {
                                builder.Append("\"Manufacturer\" = @ManufactureValue,");
                                command.Parameters.AddWithValue("@ManufactureValue", newCar.Manufacturer);

                            }
                            if (!string.IsNullOrEmpty(newCar.Model))
                            {
                                builder.Append("\"Model\" = @ModelValue,");
                                command.Parameters.AddWithValue("@ModelValue", newCar.Model);

                            }
                            if (newCar.NumberOfSeats != 0)
                            {
                                builder.Append("\"NumberOfSeats\" = @NumberOfSeatsValue,");
                                command.Parameters.AddWithValue("@NumberOfSeatsValue", newCar.NumberOfSeats);
                            }
                            if (newCar.Price != 0)
                            {
                                builder.Append("\"Price\" = @PriceValue,");
                                command.Parameters.AddWithValue("@PriceValue", newCar.Price);
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
                            command.Parameters.AddWithValue("@OldIdValue", oldCar.Id);
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

        public int DeleteCar(Guid id)
        {
            Car carToDelete = this.GetCarById(id);
            int affectedRows = 0;
            try
            {
                if (carToDelete != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM \"Car\" WHERE \"Id\" = @IdValue";
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

        private Car GetCarById(Guid id)
        {
            Car car = null;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"Car\" WHERE \"Id\" = @Value";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value", id);
                        NpgsqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            car = new Car();
                            reader.Read();
                            car.Id = (Guid)reader["Id"];
                            car.Manufacturer = (string)reader["Manufacturer"];
                            car.Model = (string)reader["Model"];
                            car.NumberOfSeats = (int)reader["NumberOfSeats"];
                            car.Price = (double)reader["Price"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return car;
        }
    }
}
