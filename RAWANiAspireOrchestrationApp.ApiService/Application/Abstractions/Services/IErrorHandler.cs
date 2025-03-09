using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services
{
    public interface IErrorHandler
    {
        OperationResult<T> HandleException<T>(Exception ex);
        OperationResult<T> HandleCancelationToken<T>(OperationCanceledException ex);
    }
}
