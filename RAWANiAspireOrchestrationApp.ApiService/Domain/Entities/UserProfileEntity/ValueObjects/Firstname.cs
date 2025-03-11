using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects
{
    public readonly struct Firstname : IEquatable<Firstname>
    {
        public string Value { get; }
        public const int MaxLength = 50;
        private Firstname(string value) => Value = value;
        public static OperationResult<Firstname> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return OperationResult<Firstname>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The Firstname is required, please provide a valid value."
                );
            }

            if (value.Length > MaxLength)
            {
                return OperationResult<Firstname>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    $"The Firstname must not exceed {MaxLength} characters."
                );
            }

            return OperationResult<Firstname>.Success(new Firstname(value));
        }
        public override string ToString() => Value;
        public bool Equals(Firstname other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is Firstname other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Firstname left, Firstname right) => left.Equals(right);
        public static bool operator !=(Firstname left, Firstname right) => !left.Equals(right);
        public static implicit operator Firstname(string value) => new(value);
        public static implicit operator string(Firstname names) => names.Value;
        //public static Firstname CreateNew() => new(string.Empty);
        public void Deconstruct(out string value) => value = Value;
    }
}
