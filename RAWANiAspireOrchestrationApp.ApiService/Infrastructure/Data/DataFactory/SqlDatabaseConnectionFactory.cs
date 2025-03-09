using Microsoft.Data.SqlClient;

namespace RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory
{
    public class SqlDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public async Task<IDatabaseConnection> CreateConnectionAsync(string connectionString,
            CancellationToken cancellationToken)
        {
            var connection = new SqlConnection(connectionString);
            return new SqlDatabaseConnection(connection);
        }
    }
}
