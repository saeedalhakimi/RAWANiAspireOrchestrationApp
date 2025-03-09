namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Models
{
    public class Error
    {
        public ErrorCode Code { get; }
        public string Message { get; }
        public string? Details { get; }
        public Error(ErrorCode code, string message, string? details = null)
        {
            Code = code;
            Message = message;
            Details = details;
        }
        public override string ToString() =>
            string.IsNullOrEmpty(Details)
                ? $"Error {Code}: {Message}"
                : $"Error {Code}: {Message} - Details: {Details}";

        public string ToLogString(bool includeDetails = true) =>
            includeDetails && !string.IsNullOrEmpty(Details)
                ? $"Code: {Code}, Message: {Message}, Details: {Details}"
                : $"Code: {Code}, Message: {Message}";

        public Dictionary<string, object?> ToDictionary() =>
            new Dictionary<string, object?>
            {
                { "Code", Code },
                { "Message", Message },
                { "Details", Details }
            };
    }
}
