using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Queries;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.QueryHandlers
{
    public class GetUserProfileByUserProfileIDQueryHandler : IRequestHandler<GetUserProfileByUserProfileIDQuery, OperationResult<UserProfileResponseDto>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger<GetUserProfileByUserProfileIDQueryHandler> _logger;
        private readonly IErrorHandler _errorHandler;
        public GetUserProfileByUserProfileIDQueryHandler(IUserProfileRepository userProfileRepository, ILogger<GetUserProfileByUserProfileIDQueryHandler> logger, IErrorHandler errorHandler)
        {
            _userProfileRepository = userProfileRepository;
            _logger = logger;
            _errorHandler = errorHandler;
        }
        public async Task<OperationResult<UserProfileResponseDto>> Handle(GetUserProfileByUserProfileIDQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserProfileByUserProfileIDQuery... ");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var userProfile = await _userProfileRepository.GetUserProfileByUserProfileIDAsync(request.UserProfileID, cancellationToken);
                if (!userProfile.IsSuccess)
                {
                    _logger.LogError("Error occurred while getting user profile by user profile ID. Error: {error}", userProfile.Errors);
                    return OperationResult<UserProfileResponseDto>.Failure(userProfile.Errors);
                }

                var response = UserProfileMappers.ToUserProfileResponseDto(userProfile.Data!);
                _logger.LogInformation("User profile mapped successfully... ");
                return OperationResult<UserProfileResponseDto>.Success(response);
            }
            catch (OperationCanceledException ex)
            {
                return _errorHandler.HandleCancelationToken<UserProfileResponseDto>(ex);
            }
            catch (Exception ex)
            {
                return _errorHandler.HandleException<UserProfileResponseDto>(ex);
            }
        }
    }
}
