using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PGViewer.Repository
{
    public abstract class RepositoryBase
    {
        public RepositoryBase()
        {
        }

        protected NpgsqlConnection GetConnection() {
            return new NpgsqlConnection($"Host=localhost;Database=mydb;Username=admin;Password=admin123");
        }
    }
}
