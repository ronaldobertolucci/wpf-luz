using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_luz.Database
{
    public class ConnectionFactory
    {
        private readonly string connectionString;
        private NpgsqlConnection connection;

        public ConnectionFactory(string host, string port, string username, string pass)
        {
            connectionString = $"Host={host};Port={port};Username={username};Password={pass};";
        }

        public NpgsqlConnection getConnection()
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    Trace.WriteLine("Connection opened");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return connection;
        }
    }
}
