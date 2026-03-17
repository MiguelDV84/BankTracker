using Dapper;
using MySqlConnector;
using System.Data;

namespace BankTrackerApi.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection CreateConnection()
            => new MySqlConnection(_connectionString);
    }
}
