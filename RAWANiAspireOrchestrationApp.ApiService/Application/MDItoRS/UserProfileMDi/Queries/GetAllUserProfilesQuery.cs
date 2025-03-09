using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Application.Models;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Queries
{
    public class GetAllUserProfilesQuery 
        : IRequest<OperationResult<PagedResponse<UserProfileResponseDto>>>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}
