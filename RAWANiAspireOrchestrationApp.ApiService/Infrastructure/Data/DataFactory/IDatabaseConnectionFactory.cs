namespace RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDatabaseConnection> CreateConnectionAsync(string connectionString, CancellationToken cancellationToken);
    }
}
