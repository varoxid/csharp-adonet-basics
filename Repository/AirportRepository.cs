using PGViewer.Model;
using Npgsql;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Windows.Documents;

namespace PGViewer.Repository
{
    public class AirportRepository : RepositoryBase, IAirportRepository
    {

        public (List<AirportModel> Airports, int TotalCount) GetAll(int pageNumber, int pageSize) 
        {
            var airports = new List<AirportModel>();
            int totalCount = 0;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM Airports", connection))
                {
                    totalCount = Convert.ToInt32(countCommand.ExecuteScalar());
                }

                string query = @"
                    SELECT id, airport, city, country, iata, icao, region 
                    FROM Airports 
                    ORDER BY airport
                    LIMIT @PageSize OFFSET @Offset";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airports.Add(new AirportModel
                            {
                                Id = reader.GetInt32(0),
                                Airport = reader.GetString(1),
                                City = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Country = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Iata = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Icao = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Region = reader.IsDBNull(6) ? null : reader.GetString(6)
                            });
                        }
                    }
                }
            }

            return (airports, totalCount);
        }

        public void UpdateAirport(AirportModel airport)
        {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    string query = @"
                    UPDATE Airports 
                    SET 
                        airport = @Airport,
                        city = @City,
                        country = @Country,
                        iata = @Iata,
                        icao = @Icao,
                        region = @Region
                    WHERE id = @Id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Airport", airport.Airport);
                        command.Parameters.AddWithValue("@City", airport.City ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Country", airport.Country ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Iata", airport.Iata ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Icao", airport.Icao ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Region", airport.Region);
                        command.Parameters.AddWithValue("@Id", airport.Id);

                        command.ExecuteNonQuery();
                    }
                }
        }

        public void DeleteAirport(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Airports WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int AddAirport(AirportModel airport)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    INSERT INTO Airports 
                        (id, airport, city, country, iata, icao, region)
                    VALUES 
                        (@Id, @Airport, @City, @Country, @Iata, @Icao, @Region)";
                var airlineId = GenerateAirportId(connection);

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", airlineId);
                    command.Parameters.AddWithValue("@Airport", airport.Airport);
                    command.Parameters.AddWithValue("@City", airport.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Country", airport.Country ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Iata", airport.Iata ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Icao", airport.Icao ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Region", airport.Region);

                    command.ExecuteScalar();
                    return airlineId;
                }
            }
        }

        private int GenerateAirportId(NpgsqlConnection connection)
        {
            string query = "SELECT COALESCE(MAX(id), 0) FROM Airports";
            using (var command = new NpgsqlCommand(query, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar()) + 1;
            }
        }
    }
}