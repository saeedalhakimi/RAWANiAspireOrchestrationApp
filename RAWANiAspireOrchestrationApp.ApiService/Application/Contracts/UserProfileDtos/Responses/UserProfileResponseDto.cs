namespace RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses
{
    public record UserProfileResponseDto
    {
        public Guid UserProfileID { get; set; }
        public string UserID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentCity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
