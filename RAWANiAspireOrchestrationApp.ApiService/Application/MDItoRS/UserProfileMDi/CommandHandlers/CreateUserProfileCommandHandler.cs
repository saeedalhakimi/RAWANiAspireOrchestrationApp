using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Commands;
using RAWANiAspireOrchestrationApp.ApiService.Application.Services;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Intities.UserProfileIntity;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.CommandHandlers
{
    public class CreateUserProfileCommandHandler
        : IRequestHandler<CreateUserProfileCommand, OperationResult<UserProfileResponseDto>>
    {
        private readonly ILogger<CreateUserProfileCommandHandler> _logger;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IErrorHandler _errorHandler;
        public CreateUserProfileCommandHandler(
            ILogger<CreateUserProfileCommandHandler> logger,
            IUserProfileRepository userProfileRepository,
            IErrorHandler errorHandlingService)
        {
            _logger = logger;
            _userProfileRepository = userProfileRepository;
            _errorHandler = errorHandlingService;
        }
        public async Task<OperationResult<UserProfileResponseDto>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateUserProfileCommand... ");

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Step 1: Check if user profile already exists
                var userProfileExists = await _userProfileRepository.IsUserProfileExistsByEmailAsync(request.Email, cancellationToken);
                if (!userProfileExists.IsSuccess)
                {
                    _logger.LogError("Error occurred while checking if user profile exists by email. Error: {error}", userProfileExists.Errors);
                    return OperationResult<UserProfileResponseDto>.Failure(userProfileExists.Errors);
                }

                if (userProfileExists.Data)
                {
                    _logger.LogError("User profile already exists with the provided email address {email}", request.Email);
                    return OperationResult<UserProfileResponseDto>.Failure(
                        ErrorCode.Conflict,
                        "USER_PROFILE_ALREADY_EXISTS",
                        $"User profile already exists with the provided email address {request.Email}.");
                }
                    
                //Step 2: Create user 
                var userId = Guid.NewGuid().ToString();

                //Step 3: create user profile
                var userProfile = UserProfile.Create(Guid.NewGuid(), userId, request.Firstname, request.Lastname, request.Email, request.DateOfBirth ,request.PhoneNumber, request.CurrentCity, null, null);
                if (!userProfile.IsSuccess) 
                    return OperationResult<UserProfileResponseDto>.Failure(userProfile.Errors);
                _logger.LogInformation("User profile created successfully... ");

                //Step 4: Save user profile to database
                var saveUserProfile = await _userProfileRepository.CreateUserProfileAsync(userProfile.Data!, cancellationToken);
                if (!saveUserProfile.IsSuccess)
                {
                    _logger.LogError("Error occurred while saving user profile. Error: {error}", saveUserProfile.Errors);
                    return OperationResult<UserProfileResponseDto>.Failure(saveUserProfile.Errors);
                }
                //Step 5: Map and Return response
                var response = UserProfileMappers.ToUserProfileResponseDto(userProfile.Data!);
                
                _logger.LogInformation("CreateUserProfileCommand handled successfully... ");
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
