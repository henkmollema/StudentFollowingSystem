using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vul een e-mailadres in.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vul een geldig e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vul een wachtwoord in.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
