using Microsoft.Data.SqlClient.Diagnostics;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Intities.UserProfileIntity;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;
using RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory;
using System.Data;

namespace RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Repository.UserProfileRepo
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly IErrorHandler _errorHandler;
        private readonly ILogger<UserProfileRepository> _logger;
        private readonly string _connectionString;
        public UserProfileRepository(
            IConfiguration configuration, IDatabaseConnectionFactory connectionFactory,
            IErrorHandler errorHandler, ILogger<UserProfileRepository> logger,string connectionString = null)
        {
            _connectionString = connectionString ?? configuration.GetConnectionString("DefaultConnection")!;
            _connectionFactory = connectionFactory;
            _errorHandler = errorHandler;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saving user profile to Database...");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogDebug("Cancellation token checked. Proceeding with database connection.");

                await using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString, cancellationToken);
                using var command = connection.CreateCommand();
                command.CommandText = "SP_CreateUserProfile";
                command.CommandType = CommandType.StoredProcedure;
                command.AddParameter("@UserProfileID", userProfile.UserProfileID);
                command.AddParameter("@UserID", userProfile.UserID);
                command.AddParameter("@Firstname", userProfile.BasicInfo.Firstname);
                command.AddParameter("@Lastname", userProfile.BasicInfo.Lastname);
                command.AddParameter("@Email", userProfile.BasicInfo.Email);
                command.AddParameter("@DateOfBirth", userProfile.BasicInfo.DateOfBirth);
                command.AddParameter("@PhoneNumber", userProfile.BasicInfo.PhoneNumber ?? (object)DBNull.Value);
                command.AddParameter("@CurrentCity", userProfile.BasicInfo.CurrentCity ?? (object)DBNull.Value);
                command.AddParameter("@CreatedAt", userProfile.CreatedAt);
                command.AddParameter("@UpdatedAt", userProfile.UpdatedAt);
                command.AddOutputParameter("@RowsAffected", SqlDbType.Int);

                await connection.OpenAsync(cancellationToken);
                _logger.LogInformation("Database connection opened. Executing command...");

                _logger.LogInformation("Executing Stored Procedure '{StoredProcedure}' to save the user profile. ", command.CommandText);
                await command.ExecuteNonQueryAsync(cancellationToken);

                int rowsAffected = Convert.ToInt32(command.GetParameterValue("@RowsAffected"));
                if (rowsAffected > 0)
                {
                    _logger.LogInformation("User profile saved successfully. Rows affected: {rowsAffected}", rowsAffected);
                    return OperationResult<bool>.Success(true);
                }
                else
                {
                    _logger.LogError("Error occurred while saving user profile. Rows affected: {rowsAffected}", rowsAffected);
                    return OperationResult<bool>.Failure(ErrorCode.InternalError, "USER_PROFILE_SAVE_ERROR", "Error occurred while saving user profile.");
                }
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
        public async Task<OperationResult<int>> GetUserProfilesCountAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving User Profiles total count from Database...");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogDebug("Cancellation token checked. Proceeding with database connection.");

                await using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString, cancellationToken);
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM UserProfile WHERE IsDeleted = 0";
                command.CommandType = CommandType.Text;

                await connection.OpenAsync(cancellationToken);
                _logger.LogInformation("Database connection opened. Executing query...");

                _logger.LogInformation("Executing query '{query}' to get the total count of user profiles. ", command.CommandText);
                var result = await command.ExecuteScalarAsync(cancellationToken);

                if (result != null && int.TryParse(result.ToString(), out int userProfilesCount))
                {
                    _logger.LogInformation("Successfully retrieved user profiles count: {userProfilesCount}", userProfilesCount);
                    return OperationResult<int>.Success(userProfilesCount);
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve user profile count. Returning 0 count...)");
                    return OperationResult<int>.Failure(new Error(
                        ErrorCode.NotFound,
                        "USER_PROFILE_COUNT_RETRIEVE_FAILED",
                        "No user profiles found in database."
                    ));
                }
            }
            catch (OperationCanceledException ex)
            {
                return _errorHandler.HandleCancelationToken<int>(ex);
            }
            catch (Exception ex)
            {
                return _errorHandler.HandleException<int>(ex);
            }
        }
        public async Task<OperationResult<IEnumerable<UserProfile>>> GetAllUserProfilesAsync(int pageNumber, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving User Profiles from Database...");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogDebug("Cancellation token checked. Proceeding with database connection.");

                await using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString, cancellationToken);
                using var command = connection.CreateCommand();
                command.CommandText = "SP_GetAllUserProfiles";
                command.CommandType = CommandType.StoredProcedure;
                command.AddParameter("@PageNumber", pageNumber);
                command.AddParameter("@PageSize", pageSize);
                command.AddParameter("@SortColumn", sortColumn ?? "CreatedAt");
                command.AddParameter("@SortDirection", sortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC");

                await connection.OpenAsync(cancellationToken);
                _logger.LogInformation("Database connection opened. Executing query...");
                _logger.LogInformation("Executing Stored Procedure '{StoredProcedure}' to get user profiles. ", command.CommandText);

                var userProfiles = new List<UserProfile>();
                using var reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    var userProfile = UserProfile.Create(
                        reader.GetGuid(reader.GetOrdinal("UserProfileID")),
                        reader.GetString(reader.GetOrdinal("UserID")),
                        reader.GetString(reader.GetOrdinal("Firstname")),
                        reader.GetString(reader.GetOrdinal("Lastname")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                        reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                        reader.IsDBNull(reader.GetOrdinal("CurrentCity")) ? null : reader.GetString(reader.GetOrdinal("CurrentCity")),
                        reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        );
                    if (!userProfile.IsSuccess)
                    {
                        _logger.LogError("Error occurred while creating UserProfile from retrieved user profile data. Error: {error}", userProfile.Errors);
                        return OperationResult<IEnumerable<UserProfile>>.Failure(userProfile.Errors);
                    }
                    userProfiles.Add(userProfile.Data!);
                }

                _logger.LogInformation("User profiles retrieved successfully. Total profiles: {count}", userProfiles.Count);
                return OperationResult<IEnumerable<UserProfile>>.Success(userProfiles);
            }
            catch (OperationCanceledException ex)
            {
                return _errorHandler.HandleCancelationToken<IEnumerable<UserProfile>>(ex);
            }
            catch (Exception ex)
            {
                return _errorHandler.HandleException<IEnumerable<UserProfile>>(ex);
            }
        }
        public async Task<OperationResult<UserProfile>> GetUserProfileByUserProfileIDAsync(Guid userPrfileId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving profile for user profile ID {userPrfileId}...");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogDebug("Cancellation token checked. Proceeding with database connection.");

                await using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString, cancellationToken);
                using var command = connection.CreateCommand();
                command.CommandText = "SP_GetUserProfileByUserProfileID";
                command.CommandType = CommandType.StoredProcedure;
                command.AddParameter("@UserProfileID", userPrfileId);

                await connection.OpenAsync(cancellationToken);
                _logger.LogInformation("Database connection opened. Executing query...");
                _logger.LogInformation("Executing Stored Procedure '{StoredProcedure}' to get the user profile. ", command.CommandText);

                using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    var userProfile = UserProfile.Create(
                        reader.GetGuid(reader.GetOrdinal("UserProfileID")),
                        reader.GetString(reader.GetOrdinal("UserID")),
                        reader.GetString(reader.GetOrdinal("Firstname")),
                        reader.GetString(reader.GetOrdinal("Lastname")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                        reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                        reader.IsDBNull(reader.GetOrdinal("CurrentCity")) ? null : reader.GetString(reader.GetOrdinal("CurrentCity")),
                        reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        );

                    if (!userProfile.IsSuccess) { 
                        _logger.LogError("Error occurred while creating UserProfile from retrieved user profile data. Error: {error}", userProfile.Errors);
                        return OperationResult<UserProfile>.Failure(userProfile.Errors);
                    }

                    _logger.LogInformation("User profile retrieved successfully.");
                    return OperationResult<UserProfile>.Success(userProfile.Data!);
                }
                else
                {
                    _logger.LogError("User profile not found for user profile ID {userProfileId}.", userPrfileId);
                    return OperationResult<UserProfile>.Failure(ErrorCode.NotFound, "USER_PROFILE_NOT_FOUND", "User profile not found.");
                }
            }
            catch (OperationCanceledException ex)
            {
                return _errorHandler.HandleCancelationToken<UserProfile>(ex);
            }

            catch (Exception ex)
            {
                return _errorHandler.HandleException<UserProfile>(ex);
            }
        }
        public async Task<OperationResult<bool>> IsUserProfileExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if user profile exists by email");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogDebug("Cancellation token checked. Proceeding with database connection.");

                await using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString, cancellationToken);
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM UserProfile WHERE Email = @Email";
                command.CommandType = CommandType.Text;
                command.AddParameter("@Email", email);

                await connection.OpenAsync(cancellationToken);
                _logger.LogInformation("Database connection opened. Executing command...");

                _logger.LogInformation("Executing query '{query}' to get the result. ", command.CommandText);

                var count = (int)await command.ExecuteScalarAsync(cancellationToken);
               _logger.LogInformation("Query executed successfully. Result: {count}", count);

                // Return the result
                return OperationResult<bool>.Success(count > 0);
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
