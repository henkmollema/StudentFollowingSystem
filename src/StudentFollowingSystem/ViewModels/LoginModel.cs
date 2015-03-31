using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class LoginModel
    {
        //Login page with the necessary validations.
        [Display(Name = "E-mailadres")]
        [Required(ErrorMessage = "Vul een e-mailadres in.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vul een geldig e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; }

        [Display(Name = "Wachtwoord")]
        [Required(ErrorMessage = "Vul een wachtwoord in.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
