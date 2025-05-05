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
    public class AirlineRepository : RepositoryBase, IAirlineRepository
    {

        public (List<AirlineModel> Airlines, int TotalCount) GetAll(int pageNumber, int pageSize) 
        {
            var airlines = new List<AirlineModel>();
            int totalCount = 0;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM Airlines", connection))
                {
                    totalCount = Convert.ToInt32(countCommand.ExecuteScalar());
                }

                string query = @"
                    SELECT id, name, alt_name, iata, icao, callsign, country, active 
                    FROM Airlines 
                    ORDER BY name
                    LIMIT @PageSize OFFSET @Offset";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airlines.Add(new AirlineModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                AltName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                IATA = reader.IsDBNull(3) ? null : reader.GetString(3),
                                ICAO = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Callsign = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Country = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Active = reader.GetBoolean(7)
                            });
                        }
                    }
                }
            }

            return (airlines, totalCount);
        }

        public void UpdateAirline(AirlineModel airline)
        {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    string query = @"
                    UPDATE Airlines 
                    SET 
                        name = @Name,
                        iata = @IATA,
                        icao = @ICAO,
                        country = @Country,
                        active = @Active
                    WHERE id = @Id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", airline.Name);
                        command.Parameters.AddWithValue("@IATA", airline.IATA ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ICAO", airline.ICAO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Country", airline.Country ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Active", airline.Active);
                        command.Parameters.AddWithValue("@Id", airline.Id);

                        command.ExecuteNonQuery();
                    }
                }
        }

        public void DeleteAirline(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Airlines WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int AddAirline(AirlineModel airline)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    INSERT INTO Airlines 
                        (id, name, alt_name, iata, icao, callsign, country, active)
                    VALUES 
                        (@Id, @Name, @AltName, @IATA, @ICAO, @Callsign, @Country, @Active)";
                var airlineId = GenerateAirlineId(connection);

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", airlineId);
                    command.Parameters.AddWithValue("@Name", airline.Name);
                    command.Parameters.AddWithValue("@AltName", airline.AltName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IATA", airline.IATA ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ICAO", airline.ICAO ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Callsign", airline.Callsign ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Country", airline.Country ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Active", airline.Active);

                    command.ExecuteScalar();
                    return airlineId;
                }
            }
        }

        private int GenerateAirlineId(NpgsqlConnection connection)
        {
            string query = "SELECT COALESCE(MAX(id), 0) FROM Airlines";
            using (var command = new NpgsqlCommand(query, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar()) + 1;
            }
        }
    }
}