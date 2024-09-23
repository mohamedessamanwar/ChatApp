using System.ComponentModel.DataAnnotations;

namespace Chating.ViewMode
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "enter valid email ")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
