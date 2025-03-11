using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects
{
    public readonly struct DateOfBirth : IEquatable<DateOfBirth>
    {
        public DateTime Value { get; }
        private DateOfBirth(DateTime value) => Value = value;
        public static OperationResult<DateOfBirth> Create(DateTime value)
        {
            var today = DateTime.Today;
            if (value > today)
            {
                return OperationResult<DateOfBirth>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The DateOfBirth cannot be in the future, please provide a valid value."
                );
            }
            var age = today.Year - value.Year;

            if (value.Date > today.AddYears(-age)) age--;

            if (age < 18)
            {
                return OperationResult<DateOfBirth>.Failure(new Error(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "Date of birth indicates an age of less than 18 years, which is invalid"));
            }

            if (age > 125)
            {
                return OperationResult<DateOfBirth>.Failure(new Error(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "Date of birth indicates an age of more than 125 years, which is invalid."
                ));
            }

            return OperationResult<DateOfBirth>.Success(new DateOfBirth(value));
        }
        public override string ToString() => Value.ToString();
        public bool Equals(DateOfBirth other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is DateOfBirth other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(DateOfBirth left, DateOfBirth right) => left.Equals(right);
        public static bool operator !=(DateOfBirth left, DateOfBirth right) => !left.Equals(right);
        public static implicit operator DateOfBirth(DateTime value) => new(value);
        public static implicit operator DateTime(DateOfBirth dateOfBirth) => dateOfBirth.Value;
        //public static DateOfBirth CreateNew() => new(DateTime.MinValue);
        public void Deconstruct(out DateTime value) => value = Value;
    }
}
