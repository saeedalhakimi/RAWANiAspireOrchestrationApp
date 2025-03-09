namespace RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory
{
    public interface IDataReader : IDisposable
    {
        Task<bool> ReadAsync(CancellationToken cancellationToken);
        Task<bool> NextResultAsync(CancellationToken cancellationToken);
        Guid GetGuid(int ordinal);
        string GetString(int ordinal);
        DateTime GetDateTime(int ordinal);
        int GetOrdinal(string name);
        int GetInt32(int ordinal);
        long GetInt64(int ordinal); 
        bool GetBoolean(int ordinal); 
        bool IsDBNull(int ordinal);
    }
}
