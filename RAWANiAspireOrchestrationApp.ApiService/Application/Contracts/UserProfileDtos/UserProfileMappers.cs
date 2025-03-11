using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos
{
    public static class UserProfileMappers
    {
        public static UserProfileResponseDto ToUserProfileResponseDto(UserProfile userProfile)
        {
            return new UserProfileResponseDto
            {
                UserProfileID = userProfile.UserProfileID.Value,
                UserID = userProfile.UserID.Value,
                Firstname = userProfile.BasicInfo.Firstname.Value,
                Lastname = userProfile.BasicInfo.Lastname.Value,
                Email = userProfile.BasicInfo.Email.Value,
                DateOfBirth = userProfile.BasicInfo.DateOfBirth.Value,    
                PhoneNumber = userProfile.BasicInfo.PhoneNumber,
                CurrentCity = userProfile.BasicInfo.CurrentCity,
                CreatedAt = userProfile.CreatedAt,
                UpdatedAt = userProfile.UpdatedAt
            };
        }
    }
}
