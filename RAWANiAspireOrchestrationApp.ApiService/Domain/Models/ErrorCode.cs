namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Models
{
    public enum ErrorCode
    {
        UnknownError = 999,
        NotFound = 1000,
        BadRequest = 1001,
        OperationCancelled = 1002,
        Conflict = 1003,
        InternalError = 1004,
        InvalidInput = 1005,
    }
}
