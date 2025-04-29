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

namespace PGViewer.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            try {
                using (var connection = GetConnection())
                {
                    var command = new NpgsqlCommand();
                    connection.Open();
                    string storedHash = GetPasswordHash(connection, credential.UserName);
                    if (storedHash == null || !BCrypt.Net.BCrypt.Verify(credential.Password, storedHash)) 
                    {
                        return false;
                    }

                    return true;
                } 
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "28P01")
            {
                Console.WriteLine($"Invalid username or password: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return false;
            }
        }

        private string GetPasswordHash(NpgsqlConnection connection, string username)
        {
            using (var cmd = new NpgsqlCommand("SELECT password FROM Account WHERE username = @username", connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            UserModel userModel = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, username, display_name, email  FROM Account WHERE username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var reader = cmd.ExecuteReader()) 
                    {
                        if (reader.Read()) {
                            userModel = new UserModel()
                            {
                                Id = reader[0].ToString(),
                                Username = reader[1].ToString(),
                                Name = reader[2].ToString(),
                                Email = reader[3].ToString()
                            };

                        }
                    }
                        
                }
            }
            return userModel;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
