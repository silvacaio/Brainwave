using System.ComponentModel.DataAnnotations;

namespace Brainwave.API.ViewModel
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress(ErrorMessage = "The {0} field is not in a valid format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string? Password { get; set; }

    }
}
