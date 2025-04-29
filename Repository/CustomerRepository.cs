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
    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {

        public (List<CustomerModel> Customers, int TotalCount) GetAll(int pageNumber, int pageSize) 
        {
            var customers = new List<CustomerModel>();
            int totalCount = 0;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM Customer", connection))
                {
                    totalCount = Convert.ToInt32(countCommand.ExecuteScalar());
                }

                string query = @"
                    SELECT id, name, comment 
                    FROM Customer 
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
                            customers.Add(new CustomerModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Comment = reader.IsDBNull(2) ? null : reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return (customers, totalCount);
        }

        public void UpdateCustomer(CustomerModel customer)
        {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    string query = @"
                    UPDATE Customer 
                    SET 
                        name = @Name,
                        comment = @Comment
                    WHERE id = @Id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", customer.Name);
                        command.Parameters.AddWithValue("@Comment", customer.Comment ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
        }

        public void DeleteCustomer(int customerId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Customer WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", customerId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int AddCustomer(CustomerModel customer)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    INSERT INTO Customer 
                        (name, comment)
                    VALUES 
                        (@Name, @Comment)
                    RETURNING id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Comment", customer.Comment ?? (object)DBNull.Value);

                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}