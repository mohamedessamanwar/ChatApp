using System.ComponentModel.DataAnnotations;

namespace Chating.DataView
{
    public class SignupView
    {
        [Required]
        [EmailAddress (ErrorMessage = "enter valid email ")]
        public string Email {get; set;}

        [Required (ErrorMessage ="firstname is required")]
        [MaxLength (50)]
        public string Firstname {get; set;}


        [Required(ErrorMessage = "firstname is required")]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [Required]
        [DataType (DataType.Password)]
        [MaxLength (50,ErrorMessage ="Max length is 50")]
        public string Password { get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
