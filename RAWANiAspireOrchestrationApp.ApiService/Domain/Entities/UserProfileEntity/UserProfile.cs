using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity
{
    public class UserProfile
    {
        public Guid UserProfileID { get; private set; }
        public string UserID { get; private set; }
        public BasicInformation BasicInfo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private UserProfile() { }
        public static OperationResult<UserProfile> Create(
            Guid userProfileId,string userID, string firstname, string lastname, string email, DateTime dateOfBirth ,string phoneNumber, string currentCity, DateTime? createdAT, DateTime? updatedAT)
        {
            var basicInfo = BasicInformation.Create(firstname, lastname, email, dateOfBirth, phoneNumber, currentCity);
            if (!basicInfo.IsSuccess) return OperationResult<UserProfile>.Failure(basicInfo.Errors);

            var createdAt = createdAT ?? DateTime.UtcNow;
            var updatedAt = updatedAT ?? createdAt;

            return OperationResult<UserProfile>.Success(new UserProfile
            {
                UserProfileID = userProfileId,
                UserID = userID,
                BasicInfo = basicInfo.Data!,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
            });
        }
    }
}
