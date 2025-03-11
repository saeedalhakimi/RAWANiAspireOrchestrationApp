using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects
{
    public readonly struct Lastname : IEquatable<Lastname>
    {
        public string Value { get; }
        public const int MaxLength = 50;
        private Lastname(string value) => Value = value;
        public static OperationResult<Lastname> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return OperationResult<Lastname>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The Lastname is required, please provide a valid value."
                );
            }
            if (value.Length > MaxLength)
            {
                return OperationResult<Lastname>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    $"The Lastname must not exceed {MaxLength} characters."
                );
            }
            return OperationResult<Lastname>.Success(new Lastname(value));
        }
        public override string ToString() => Value;
        public bool Equals(Lastname other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is Lastname other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Lastname left, Lastname right) => left.Equals(right);
        public static bool operator !=(Lastname left, Lastname right) => !left.Equals(right);
        public static implicit operator Lastname(string value) => new(value);
        public static implicit operator string(Lastname names) => names.Value;
        //public static Lastname CreateNew() => new(string.Empty);
        public void Deconstruct(out string value) => value = Value;
    }
}
