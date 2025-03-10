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
                UserProfileID = userProfile.UserProfileID,
                UserID = userProfile.UserID,
                Firstname = userProfile.BasicInfo.Firstname,
                Lastname = userProfile.BasicInfo.Lastname,
                Email = userProfile.BasicInfo.Email,
                DateOfBirth = userProfile.BasicInfo.DateOfBirth,    
                PhoneNumber = userProfile.BasicInfo.PhoneNumber,
                CurrentCity = userProfile.BasicInfo.CurrentCity,
                CreatedAt = userProfile.CreatedAt,
                UpdatedAt = userProfile.UpdatedAt
            };
        }
    }
}
