using MediatR;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Responses;
using RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Queries;
using RAWANiAspireOrchestrationApp.ApiService.Application.Models;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.QueryHandlers
{
    public class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfilesQuery, OperationResult<PagedResponse<UserProfileResponseDto>>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger<GetAllUserProfilesQueryHandler> _logger;
        private readonly IErrorHandler _errorHandler;
        public GetAllUserProfilesQueryHandler(
            IUserProfileRepository userProfileRepository,
            ILogger<GetAllUserProfilesQueryHandler> logger,
            IErrorHandler errorHandler)
        {
            _userProfileRepository = userProfileRepository;
            _logger = logger;
            _errorHandler = errorHandler;
        }
        public async Task<OperationResult<PagedResponse<UserProfileResponseDto>>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUserProfilesQuery... ");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var userProfilesTotalCount = await _userProfileRepository.GetUserProfilesCountAsync(cancellationToken);
                if (!userProfilesTotalCount.IsSuccess)
                {
                    _logger.LogError($"Failed to get user profiles count... {userProfilesTotalCount.Errors}");
                    return OperationResult<PagedResponse<UserProfileResponseDto>>.Failure(userProfilesTotalCount.Errors);
                }

                var userProfiles = await _userProfileRepository.GetAllUserProfilesAsync(request.PageNumber, request.PageSize, request.SortColumn, request.SortDirection, cancellationToken);
                if (!userProfiles.IsSuccess)
                {
                    _logger.LogError($"Failed to get all user profiles... {userProfiles.Errors}");
                    return OperationResult<PagedResponse<UserProfileResponseDto>>.Failure(userProfiles.Errors);
                }

                var currentPageCount = userProfiles.Data?.ToList().Count ?? 0;
                var responseDto = userProfiles.Data?.Select(UserProfileMappers.ToUserProfileResponseDto).ToList() ?? new List<UserProfileResponseDto>();
                var pagedResponse = new PagedResponse<UserProfileResponseDto>(responseDto, request.PageNumber, request.PageSize, userProfilesTotalCount.Data, currentPageCount);
                _logger.LogInformation("Successfully handled GetAllUserProfilesQuery... ");
                return OperationResult<PagedResponse<UserProfileResponseDto>>.Success(pagedResponse);

            }
            catch (OperationCanceledException ex)
            {
                return _errorHandler.HandleCancelationToken<PagedResponse<UserProfileResponseDto>>(ex);
            }
            catch (Exception ex)
            {
                return _errorHandler.HandleException<PagedResponse<UserProfileResponseDto>>(ex);
            }
        }
    }
}
