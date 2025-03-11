using RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity
{
    public class UserProfile
    {
        public UserProfileGuid UserProfileID { get; private set; }
        public UserID UserID { get; private set; }
        public BasicInformation BasicInfo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private UserProfile() { }
        public static OperationResult<UserProfile> Create(
            Guid userProfileId,string userId, string firstname, string lastname, string email, DateTime dateOfBirth ,string phoneNumber, string currentCity, DateTime? createdAT, DateTime? updatedAT)
        {
            var userProfileGuid = UserProfileGuid.Create(userProfileId);
            if (!userProfileGuid.IsSuccess) return OperationResult<UserProfile>.Failure(userProfileGuid.Errors);

            var userID = UserID.Create(userId);
            if (!userID.IsSuccess) return OperationResult<UserProfile>.Failure(userID.Errors);

            var basicInfo = BasicInformation.Create(firstname, lastname, email, dateOfBirth, phoneNumber, currentCity);
            if (!basicInfo.IsSuccess) return OperationResult<UserProfile>.Failure(basicInfo.Errors);

            var createdAt = createdAT ?? DateTime.UtcNow;
            var updatedAt = updatedAT ?? createdAt;

            return OperationResult<UserProfile>.Success(new UserProfile
            {
                UserProfileID = userProfileGuid.Data,
                UserID = userID.Data,
                BasicInfo = basicInfo.Data!,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
            });
        }


    }
}
