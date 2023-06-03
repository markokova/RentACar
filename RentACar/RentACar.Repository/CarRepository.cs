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
using RentACar.Common;
using RentACar.Common.Responses;

namespace RentACar.Repository
{
    public class CarRepository : ICarRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public async Task<int> SaveCarAsync(Car car)
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
                        affectedRows = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return affectedRows;
        }
        public async Task<CarsResponse> GetCarsAsync(Paging paging, Sorting sorting, CarFiltering filtering)
        {
            List<Car> cars = new List<Car>();
            CarsResponse response = new CarsResponse();

            StringBuilder queryBuilder = new StringBuilder("SELECT * FROM \"Car\" ");
            StringBuilder countQueryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"Car\" ");

            queryBuilder = FilterResults(queryBuilder,filtering);
            countQueryBuilder = FilterResults(countQueryBuilder,filtering);
            
            queryBuilder.Append(" ORDER BY \"Car\".\"");
            queryBuilder.Append(sorting.Orderby + "\" ");
            queryBuilder.Append(sorting.SortOrder + " ");
            queryBuilder.Append("LIMIT " + paging.PageSize + " ");
            queryBuilder.Append("OFFSET " + (paging.CurrentPageNumber - 1) * (paging.PageSize - 1));            
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    //string query = "SELECT * FROM \"Car\" ORDER BY \"Car\".@OrderBy @SortOrder LIMIT @PageSize OFFSET @OffsetValue";
                    using (NpgsqlCommand command = new NpgsqlCommand(queryBuilder.ToString(), connection))
                    {
                        //if(sorting.SortOrder == "Desc" || sorting.SortOrder == "DESC" || sorting.SortOrder == "desc")
                        //{
                        //    queryBuilder.Append("WHERE \"Car\".@OrderBy > ");
                        //}
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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
                        reader.Close();
                        response.Cars = cars;
                    }
                    using (NpgsqlCommand command = new NpgsqlCommand(countQueryBuilder.ToString(), connection))
                    {
                        object countResult = await command.ExecuteScalarAsync();
                        response.TotalNumberOfResults = Convert.ToInt32(countResult);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return response;
        }

        public async Task<Car> GetCarAsync(Guid id)
        {
            return await this.GetCarByIdAsync(id);
        }

        public async Task<List<Car>> GetCarByPriceAsync(double price)
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
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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

        public async Task<int> UpdateCarAsync(Guid id, Car newCar)
        {
            Car oldCar = await this.GetCarByIdAsync(id);
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

        public async Task<int> DeleteCarAsync(Guid id)
        {
            Car carToDelete = await this.GetCarByIdAsync(id);
            int affectedRows = 0;
            try
            {
                if (carToDelete != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM \"Reservation\" WHERE \"CarId\" = @CarIdValue";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CarIdValue", id);
                            affectedRows = await command.ExecuteNonQueryAsync();
                        }
                        string query2 = "DELETE FROM \"Car\" WHERE \"Id\" = @IdValue";
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

        private async Task<Car> GetCarByIdAsync(Guid id)
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
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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

        private StringBuilder FilterResults(StringBuilder builder, CarFiltering filtering)
        {
            if (filtering.MinPrice.HasValue && filtering.MaxPrice.HasValue)
            {
                builder.Append("WHERE \"Car\".\"Price\" BETWEEN " + filtering.MinPrice + " AND " + filtering.MaxPrice);
            }
            else if (filtering.MinPrice.HasValue)
            {
                builder.Append("WHERE \"Car\".\"Price\" > " + filtering.MinPrice);
            }
            else if (filtering.MaxPrice.HasValue)
            {
                builder.Append("WHERE \"Car\".\"Price\" < " + filtering.MaxPrice);
            }
            if (filtering.NumberOfSeats.HasValue && !filtering.MaxPrice.HasValue && !filtering.MinPrice.HasValue)
            {
                builder.Append("WHERE \"Car\".\"NumberOfSeats\" = " + filtering.NumberOfSeats);
            }
            else if (filtering.NumberOfSeats.HasValue)
            {
                builder.Append(" AND \"Car\".\"NumberOfSeats\" = " + filtering.NumberOfSeats);
            }
            return builder;
        }
    }
}
