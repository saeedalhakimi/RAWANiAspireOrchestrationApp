using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger<ErrorHandler> _logger;
        public ErrorHandler(ILogger<ErrorHandler> logger)
        {
            _logger = logger;
        }
        public OperationResult<T> HandleCancelationToken<T>(OperationCanceledException ex)
        {
            _logger.LogWarning(ex, $"The operation was canceled: {ex.Message} ");
            return OperationResult<T>.Failure(new Error(
                ErrorCode.OperationCancelled,
                $"THE_OPERATION_WAS_CANCELED: {ex.Message}",
                $"{ex.Source} - {ex.ToString()}."));
        }

        public OperationResult<T> HandleException<T>(Exception ex)
        {
            _logger.LogError(ex, $"An error occurred: {ex.Message} ");
            return OperationResult<T>.Failure(new Error(
                ErrorCode.UnknownError,
                $"AN_ERROR_ACCURRED: {ex.Message}",
                $"{ex.Source} - {ex.ToString()}."));
        }
    }
}
