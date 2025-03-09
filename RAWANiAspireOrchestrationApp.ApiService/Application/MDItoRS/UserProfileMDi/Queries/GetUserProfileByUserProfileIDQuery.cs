using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Queries
{
    public class GetUserProfileByUserProfileIDQuery : IRequest<OperationResult<UserProfileResponseDto>>
    {
        public Guid UserProfileID { get; set; }
    }
}
