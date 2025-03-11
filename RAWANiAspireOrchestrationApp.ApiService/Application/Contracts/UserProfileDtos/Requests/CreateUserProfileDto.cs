using RAWANiAspireOrchestrationApp.ApiService.Application.Filters;
using System.ComponentModel.DataAnnotations;

namespace RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Requests
{
    public record CreateUserProfileDto
    {
        [Required(ErrorMessage = "Frist Name Required..")]
        [StringLength(50, ErrorMessage = "First Name must be less than 50 characters..")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last Name Required..")]
        [StringLength(50, ErrorMessage = "Last Name must be less than 50 characters..")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email Required..")]
        [EmailAddress(ErrorMessage = "Invalid Email Address..")]
        [StringLength(50, ErrorMessage = "Email must be less than 50 characters..")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth Required..")]
        [AgeRange(18, 125)] // Validate age is between 18 and 125 years
        public DateTime DateOfBirth { get; set; }

        [StringLength(20, ErrorMessage = "Phone Number must be less than 50 characters..")]
        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "City Name must be less than 50 charaters..")]
        public string CurrentCity { get; set; }

    }
}
