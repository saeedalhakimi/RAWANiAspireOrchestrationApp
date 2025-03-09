using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Intities.UserProfileIntity.ValueObjects
{
    public readonly struct UserProfileGuid : IEquatable<UserProfileGuid>
    {
        public Guid Value { get; }
        private UserProfileGuid(Guid value) => Value = value;
        public static OperationResult<UserProfileGuid> Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                return OperationResult<UserProfileGuid>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The UserProfileID is required, please provid a valid identifier."
                );
            }
                
            return OperationResult<UserProfileGuid>.Success(new UserProfileGuid(value));
        }
        public bool Equals(UserProfileGuid other)
        {
            throw new NotImplementedException();
        }
    }
}
