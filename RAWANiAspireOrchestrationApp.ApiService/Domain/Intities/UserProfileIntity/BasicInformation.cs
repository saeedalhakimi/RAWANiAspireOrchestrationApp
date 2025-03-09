using RAWANiAspireOrchestrationApp.ApiService.Domain.Models;

namespace RAWANiAspireOrchestrationApp.ApiService.Domain.Intities.UserProfileIntity
{
    public class BasicInformation
    {
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; }
        public string CurrentCity { get; private set; }

        private BasicInformation() { }
        public static OperationResult<BasicInformation> Create(
            string firstname, string lastname, string email, DateTime dateOfBirth, string phoneNumber, string currentCity)
        {
            return OperationResult<BasicInformation>.Success(new BasicInformation
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                CurrentCity = currentCity
            });
        }
    }
}
