using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Commands;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.CommandHandlers
{
    public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<bool>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger<DeleteUserProfileCommandHandler> _logger;
        private readonly IErrorHandler _errorHandler;
        public DeleteUserProfileCommandHandler(IUserProfileRepository userProfileRepository,ILogger<DeleteUserProfileCommandHandler> logger,IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            _logger = logger;
            _userProfileRepository = userProfileRepository;
        }
        public async Task<OperationResult<bool>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteUserProfileCommandHandler.Handle - Deleting user profile with ID: {UserProfileId}", request.UserProfileId);
            try
            {
                var isUserProfileExists = await _userProfileRepository.IsUserProfileExistsAsync(request.UserProfileId, cancellationToken);
                if (!isUserProfileExists.IsSuccess)
                {
                    _logger.LogError("Error checking if user profile exists with ID: {UserProfileId}", request.UserProfileId);
                    return OperationResult<bool>.Failure(isUserProfileExists.Errors);
                }

                if (!isUserProfileExists.Data)
                {
                    _logger.LogError("User profile with ID: {UserProfileId} does not exist", request.UserProfileId);
                    return OperationResult<bool>.Failure(ErrorCode.NotFound,"USER_PROFILE_NOT_FOUND" ,$"User profile with ID: {request.UserProfileId} does not exist");
                }

                var result = await _userProfileRepository.DeleteUserProfileAsync(request.UserProfileId, cancellationToken);
                if (!result.IsSuccess)
                {
                    _logger.LogError("Error deleting user profile with ID: {UserProfileId}", request.UserProfileId);
                    return OperationResult<bool>.Failure(result.Errors);
                }

                _logger.LogInformation("DeleteUserProfileCommandHandler.Handle - User profile with ID: {UserProfileId} deleted successfully", request.UserProfileId);
                return OperationResult<bool>.Success(true);
            }
            catch (OperationCanceledException ex)
            {
                return _errorHandler.HandleCancelationToken<bool>(ex);
            }
            catch (Exception ex)
            {
                return _errorHandler.HandleException<bool>(ex);
            }

        }
    }
}
