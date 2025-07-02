using System.ComponentModel.DataAnnotations;

namespace Brainwave.API.ViewModel
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress(ErrorMessage = "The {0} field is not in a valid format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 8)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
