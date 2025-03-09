namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Models
{
    public class OperationResult<T>
    {
        public T? Data { get; private set; }
        public bool IsError { get; private set; }
        public bool IsSuccess => !IsError;
        public List<Error> Errors { get; private set; } = new List<Error>();
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
        private OperationResult(T payload)
        {
            Data = payload;
            IsError = false;
        }
        private OperationResult(List<Error> errors)
        {
            IsError = true;
            Errors = errors;
        }
        public static OperationResult<T> Success(T payload) => new OperationResult<T>(payload);
        public static OperationResult<(IEnumerable<T1>, T2)> Success<T1, T2>(IEnumerable<T1> item1, T2 item2)
            => new OperationResult<(IEnumerable<T1>, T2)>((item1, item2));
        public static OperationResult<T> Failure(List<Error> errors) => new OperationResult<T>(errors);
        public static OperationResult<T> Failure(Error error) => new OperationResult<T>(new List<Error> { error });
        public static OperationResult<T> Failure(ErrorCode code, string message) =>
            new OperationResult<T>(new List<Error> { new Error(code, message) });
        public static OperationResult<T> Failure(ErrorCode code, string message, string? details) =>
            new OperationResult<T>(new List<Error> { new Error(code, message, details) });
        public bool HasErrors() => IsError && Errors.Any();
        public string GetErrorMessage() => string.Join(", ", Errors.Select(e => e.Message));
        public string GetFirstErrorMessage() => Errors.FirstOrDefault()?.Message ?? string.Empty;

        public override string ToString() =>
            IsError
                ? $"Error(s) occurred at {Timestamp}: {GetErrorMessage()}"
                : $"Success at {Timestamp}: {Data}";
    }
}
