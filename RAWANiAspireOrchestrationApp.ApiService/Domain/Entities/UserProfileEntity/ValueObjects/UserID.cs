using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects
{
    public readonly struct UserID : IEquatable<UserID>
    {
        public string Value { get; }
        private UserID(string value) => Value = value;
        public static OperationResult<UserID> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return OperationResult<UserID>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The UserID is required, please provide a valid identifier."
                );
            }
            return OperationResult<UserID>.Success(new UserID(value));
        }
        public override string ToString() => Value;
        public bool Equals(UserID other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is UserID other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(UserID left, UserID right) => left.Equals(right);
        public static bool operator !=(UserID left, UserID right) => !left.Equals(right);
        public static implicit operator UserID(string value) => new(value);
        public static implicit operator string(UserID userId) => userId.Value;
        //public static UserID CreateNew() => new(string.Empty);
        public void Deconstruct(out string value) => value = Value;
    }
}
