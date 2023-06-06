using Npgsql;
using RentACar.Common;
using RentACar.Common.Responses;
using RentACar.Model;
using RentACar.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public async Task<int> SaveReservationAsync(Reservation reservation)
        {  
            int affectedRows = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO \"Reservation\" (\"Id\", \"ReservationDate\", \"CarId\", \"PersonId\") VALUES (@Value1, @Value2, @Value3, @Value4)";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", reservation.Id);
                        command.Parameters.AddWithValue("@Value2", reservation.ReservationDate);
                        command.Parameters.AddWithValue("@Value3", reservation.CarId);
                        command.Parameters.AddWithValue("@Value4", reservation.PersonId);
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

        public async Task<PagedList<Reservation>> GetReservationsAsync(Sorting sorting, Paging paging, ReservationFiltering filtering)
        {
            List<Reservation> queriedReservations = new List<Reservation>();
            int resultsTotalNumber = 0;

            StringBuilder queryBuilder = new StringBuilder("SELECT r.\"Id\" AS ReservationId, r.\"ReservationDate\", c.\"Id\" AS CarId, c.\"Manufacturer\", c.\"Model\", c.\"NumberOfSeats\", c.\"Price\", p.\"Id\" AS PersonId, p.\"FirstName\", p.\"LastName\", p.\"Email\" " +
                        "FROM \"Reservation\" r " +
                        "INNER JOIN \"Car\" c ON r.\"CarId\" = c.\"Id\" " +
                        "INNER JOIN \"Person\" p ON r.\"PersonId\" = p.\"Id\"");
            StringBuilder queryResultCount = new StringBuilder("SELECT  COUNT(*) " +
                        "FROM \"Reservation\" r " +
                        "INNER JOIN \"Car\" c ON r.\"CarId\" = c.\"Id\" " +
                        "INNER JOIN \"Person\" p ON r.\"PersonId\" = p.\"Id\"");

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        queryBuilder = FilterResults(queryBuilder, filtering);
                        queryBuilder = SortResults(queryBuilder, sorting);
                        queryBuilder = PageResults(queryBuilder);

                        command.Connection = connection;
                        command.CommandText = queryBuilder.ToString();

                        command.Parameters.AddWithValue("@OffsetValue", ((paging.CurrentPageNumber - 1) * (paging.PageSize - 1)));
                        command.Parameters.AddWithValue("@LimitValue", paging.PageSize);
                        command.Parameters.AddWithValue("@FromDate", filtering.FromDate);
                        command.Parameters.AddWithValue("@ToDate", filtering.ToDate);
                        
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Reservation reservation = new Reservation(); Car car = new Car(); Person person = new Person();
                                reservation.Id = (Guid)reader["ReservationId"];
                                reservation.ReservationDate = (DateTime)reader["ReservationDate"];
                                reservation.CarId = (Guid)reader["CarId"];
                                reservation.PersonId = (Guid)reader["PersonId"];
                                car.Id = (Guid)reader["CarId"];
                                car.Manufacturer = (string)reader["Manufacturer"];
                                car.Model = (string)reader["Model"];
                                car.NumberOfSeats = (int)reader["NumberOfSeats"];
                                car.Price = (double)reader["Price"];
                                person.Id = (Guid)reader["PersonId"];
                                person.FirstName = (string)reader["FirstName"];
                                person.LastName = (string)reader["LastName"];
                                person.Email = (string)reader["Email"];

                                if(reservation.Id != Guid.Empty)
                                {
                                    reservation.Car = car; reservation.Person = person;
                                    queriedReservations.Add(reservation);
                                }
                            }
                        }
                        reader.Close();
                    }

                    queryResultCount = FilterResults(queryResultCount, filtering);

                    using (NpgsqlCommand command = new NpgsqlCommand(queryResultCount.ToString(), connection))
                    {
                        object countResult = await command.ExecuteScalarAsync();
                        resultsTotalNumber = Convert.ToInt32(countResult);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            PagedList<Reservation> reservations = new PagedList<Reservation>(queriedReservations,resultsTotalNumber,paging.CurrentPageNumber,paging.PageSize);

            return reservations;
        }

        public async Task<int> UpdateReservationAsync(Guid id, Reservation newReservation)
        {
            Reservation oldReservation = await this.GetReservationByIdAsync(id);
            int affectedRows = 0;
            StringBuilder builder = new StringBuilder("UPDATE \"Reservation\" SET ");

            try
            {
                if (oldReservation != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            if (newReservation.ReservationDate != DateTime.MinValue)
                            {
                                builder.Append("\"ReservationDate\" = @ReservationDateValue,");
                                command.Parameters.AddWithValue("@ReservationDateValue", newReservation.ReservationDate);

                            }
                            if (newReservation.CarId != Guid.Empty)
                            {
                                builder.Append("\"CarId\" = @CarIdValue,");
                                command.Parameters.AddWithValue("@CarIdValue", newReservation.CarId);

                            }
                            if (newReservation.PersonId != Guid.Empty)
                            {
                                builder.Append("\"PersonId\" = @PersonIdValue,");
                                command.Parameters.AddWithValue("@PersonIdValue", newReservation.PersonId);
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
                            command.Parameters.AddWithValue("@OldIdValue", oldReservation.Id);
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

        public async Task<int> DeleteReservationAsync(Guid id)
        {
            int affectedRows = 0;
            try
            {
                Reservation ReservationToDelete = await this.GetReservationByIdAsync(id);
                if (ReservationToDelete != null)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM \"Reservation\" WHERE \"Id\" = @IdValue";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
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


        private async Task<Reservation> GetReservationByIdAsync(Guid id)
        {
            Reservation reservation = null;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"Reservation\" WHERE \"Id\" = @ReservationId";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationId", id);
                        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reservation = new Reservation();
                            reader.Read();
                            reservation.Id = (Guid)reader["Id"];
                            reservation.ReservationDate = (DateTime)reader["ReservationDate"];
                            reservation.CarId = (Guid)reader["CarId"];
                            reservation.PersonId = (Guid)reader["PersonId"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return reservation;
        }

        private StringBuilder FilterResults(StringBuilder builder, ReservationFiltering filtering)
        {
            builder.Append(" WHERE 1 = 1");
            
            if(filtering != null)
            {
                if (filtering.FromDate.HasValue)
                {
                    builder.Append(" AND r.\"ReservationDate\" >= @FromDate");

                }
                if (filtering.ToDate.HasValue)
                {
                    builder.Append(" AND  r.\"ReservationDate\" < @ToDate");

                }
            }
            return builder;
        }

        private StringBuilder SortResults(StringBuilder builder, Sorting sorting)
        {
            builder.Append($" ORDER BY r.\"{sorting.Orderby}\" ");
            if(sorting.SortOrder == "DESC")
            {
                builder.Append("DESC");
            }
            else
            {
                builder.Append("ASC");
            }
            return builder;
        }

        private StringBuilder PageResults(StringBuilder builder)
        {
            builder.Append(" OFFSET @OffsetValue");
            builder.Append(" LIMIT @LimitValue");
            return builder;
        }
    }
}