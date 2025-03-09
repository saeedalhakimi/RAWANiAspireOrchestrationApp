using Microsoft.AspNetCore.Mvc;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;
using RAWANiAspireOrchestrationApp.ApiService.Presentation.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Presentation.Controllers.V1
{
    public class BaseController<T> : Controller
    {
        private readonly ILogger<T> _logger;
        public BaseController(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // Dictionary for status code mappings
        private static readonly Dictionary<ErrorCode, (int StatusCode, string StatusPhrase)> StatusMappings = new()
        {
             // in use
            { ErrorCode.UnknownError, (500, "Internal Server Error")}, // Indicates an unexpected error on the server
            { ErrorCode.NotFound, (404, "Not Found")}, // Indicates that the requested resource could not be found
            { ErrorCode.BadRequest, (400, "Bad Request")},
            { ErrorCode.OperationCancelled, (499, "Client Closed Request")}, // Indicates that the client closed the request before the server could respond
            { ErrorCode.Conflict, (409, "Conflict")}, // Indicates that the request could not be completed due to a conflict with the current state of the resource
            { ErrorCode.InternalError, (500, "Internal Server Error")}, // Indicates an unexpected error on the server
            { ErrorCode.InvalidInput, (400, "Bad Request")}, // Indicates that the request could not be completed due to invalid input
        };

        protected IActionResult HandleErrorResponse<T>
            (OperationResult<T> result)
        {
            try
            {
                if (result == null || result.Errors == null || !result.Errors.Any())
                {
                    _logger.LogError("Operation result is null or contains no errors for request {Path}", HttpContext.Request.Path);
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unknown error occurred.");
                }

                var errorMessages = result.Errors.Select(e => e.Message).ToList();
                var errorDetails = result.Errors.Select(e => e.Details).ToList();

                _logger.LogError($"Errors occurred: {string.Join(", ", errorMessages)}. CorrelationId: {HttpContext.TraceIdentifier}");

                var errorCode = result.Errors.FirstOrDefault()?.Code ?? ErrorCode.UnknownError;
                var statusCode = StatusMappings.ContainsKey(errorCode) ? StatusMappings[errorCode].StatusCode : 500;
                var statusPhrase = StatusMappings.ContainsKey(errorCode) ? StatusMappings[errorCode].StatusPhrase : "Internal Server Error";


                // Construct the error response object
                var apiError = new ErrorResponse
                {
                    Timestamp = result.Timestamp,
                    CorrelationId = HttpContext.TraceIdentifier,
                    Errors = errorMessages,
                    ErrorsDetails = errorDetails!,
                    StatusCode = statusCode,
                    StatusPhrase = statusPhrase,
                    Path = HttpContext.Request.Path,
                    Method = HttpContext.Request.Method,
                    Detail = $"An error occurred while processing the request. {statusPhrase}"
                };

                return StatusCode(apiError.StatusCode, apiError);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error in HandleErrorResponse for request {Path}", HttpContext.Request.Path);

                // Return an internal server error
                var apiError = new ErrorResponse
                {
                    Timestamp = DateTime.UtcNow,
                    CorrelationId = HttpContext.TraceIdentifier,
                    StatusCode = 500,
                    StatusPhrase = "INTERNAL_SERVER_ERROR",
                    Path = HttpContext.Request.Path,
                    Method = HttpContext.Request.Method,
                    Detail = "An unexpected error occurred. Please try again later.",
                    Errors = new List<string> { ex.Message }
                };

                return StatusCode(apiError.StatusCode, apiError);
            }
        }

    }
}
