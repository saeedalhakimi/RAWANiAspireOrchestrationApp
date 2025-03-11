using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;
using System.Text.RegularExpressions;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects
{
    public readonly struct Emails : IEquatable<Emails>
    {
        public string Value { get; }
        public const int MaxLength = 50;

        private Emails(string value) => Value = value;
        public static OperationResult<Emails> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return OperationResult<Emails>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The Email is required, please provide a valid value."
                );
            }
            if (value.Length > MaxLength)
            {
                return OperationResult<Emails>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    $"The Email must not exceed {MaxLength} characters."
                );
            }

            if (!EmailRegex.IsMatch(value))
            {
                return OperationResult<Emails>.Failure(
                    ErrorCode.InvalidInput,
                    "INVALID_INPUT",
                    "The Email is invalid, please provide a valid email address."
                );
            }

            return OperationResult<Emails>.Success(new Emails(value));
        }
        public override string ToString() => Value;
        public bool Equals(Emails other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is Emails other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Emails left, Emails right) => left.Equals(right);
        public static bool operator !=(Emails left, Emails right) => !left.Equals(right);
        public static implicit operator Emails(string value) => new(value);
        public static implicit operator string(Emails email) => email.Value;
        //public static Emails CreateNew() => new(string.Empty);
        public void Deconstruct(out string value) => value = Value;
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
