using RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity.ValueObjects;
using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Entities.UserProfileEntity
{
    public class BasicInformation
    {
        public Firstname Firstname { get; private set; }
        public Lastname Lastname { get; private set; }
        public Emails Email { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; }
        public string CurrentCity { get; private set; }

        private BasicInformation() { }
        public static OperationResult<BasicInformation> Create(
            string firstname, string lastname, string email, DateTime dateOfBirth, string phoneNumber, string currentCity)
        {
            var firstName = Firstname.Create(firstname);
            if (!firstName.IsSuccess) return OperationResult<BasicInformation>.Failure(firstName.Errors);

            var lastName = Lastname.Create(lastname);
            if (!lastName.IsSuccess) return OperationResult<BasicInformation>.Failure(lastName.Errors);

            var emailAddress = Emails.Create(email);
            if (!emailAddress.IsSuccess) return OperationResult<BasicInformation>.Failure(emailAddress.Errors);

            var dateOFBirth = DateOfBirth.Create(dateOfBirth);
            if (!dateOFBirth.IsSuccess) return OperationResult<BasicInformation>.Failure(dateOFBirth.Errors);

            return OperationResult<BasicInformation>.Success(new BasicInformation
            {
                Firstname = firstName.Data,
                Lastname = lastName.Data,
                Email = emailAddress.Data,
                DateOfBirth = dateOFBirth.Data,
                PhoneNumber = phoneNumber,
                CurrentCity = currentCity
            });
        }
    }
}
