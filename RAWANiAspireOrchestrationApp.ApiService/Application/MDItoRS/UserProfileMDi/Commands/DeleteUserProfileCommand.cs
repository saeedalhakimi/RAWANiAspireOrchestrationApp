using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Commands
{
    public class DeleteUserProfileCommand : IRequest<OperationResult<bool>>
    {
        public Guid UserProfileId { get; set; }
    }
}
