using RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories
{
    public interface IUserProfileRepository
    {
        /// <summary>
        /// Checks if a user profile exists for the given email.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><c>true</c> if the profile exists; otherwise, <c>false</c>.</returns>
        Task<OperationResult<bool>> IsUserProfileExistsAsync(Guid userProfileID, CancellationToken cancellationToken);
        Task<OperationResult<bool>> IsUserProfileExistsByEmailAsync(string email, CancellationToken cancellationToken);
        Task<OperationResult<bool>> CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken);
        Task<OperationResult<UserProfile>> GetUserProfileByUserProfileIDAsync(Guid userPrfileId, CancellationToken cancellationToken);
        Task<OperationResult<int>> GetUserProfilesCountAsync(CancellationToken cancellationToken);
        Task<OperationResult<IEnumerable<UserProfile>>> GetAllUserProfilesAsync(int pageNumber, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken);
        Task<OperationResult<bool>> DeleteUserProfileAsync(Guid userProfileId, CancellationToken cancellationToken);
    }
}
