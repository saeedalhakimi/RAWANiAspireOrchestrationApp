using System.Data;

namespace RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory
{
    public interface IDbCommand : IDisposable
    {
        string CommandText { get; set; }
        CommandType CommandType { get; set; }
        void AddParameter(string name, object value);
        void AddOutputParameter(string name, SqlDbType type);
        object GetParameterValue(string name);
        Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken);
        Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken);
        Task<object> ExecuteScalarAsync(CancellationToken cancellationToken);
    }
}
