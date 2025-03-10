using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;
using System;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects
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
        public override string ToString() => Value.ToString();
        public bool Equals(UserProfileGuid other) => Value.Equals(other.Value);
        public override bool Equals(object? obj) => obj is UserProfileGuid other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(UserProfileGuid left, UserProfileGuid right) => left.Equals(right);
        public static bool operator !=(UserProfileGuid left, UserProfileGuid right) => !left.Equals(right);
        public static implicit operator UserProfileGuid(Guid value) => new(value);
        public static implicit operator Guid(UserProfileGuid userProfileGuid) => userProfileGuid.Value;
        public static UserProfileGuid CreateNew() => new(Guid.NewGuid());
        public void Deconstruct(out Guid value) => value = Value;
    }
}
