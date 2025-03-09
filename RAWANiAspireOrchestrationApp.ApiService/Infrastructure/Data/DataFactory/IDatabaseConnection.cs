namespace RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory
{
    public interface IDatabaseConnection : IAsyncDisposable
    {
        // Opens the database connection asynchronously
        Task OpenAsync(CancellationToken cancellationToken);

        // Creates a command associated with this connection
        IDbCommand CreateCommand();

        // Begins a new transaction asynchronously
        Task BeginTransactionAsync(CancellationToken cancellationToken);

        // Commits the current transaction asynchronously
        Task CommitTransactionAsync(CancellationToken cancellationToken);

        // Rolls back the current transaction asynchronously
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
    }
}
