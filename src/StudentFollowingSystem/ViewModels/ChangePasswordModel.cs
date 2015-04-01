using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class ChangePasswordModel : IValidatableObject
    {
        [Display(Name = "Huidig wachtwoord")]
        [Required(ErrorMessage = "Huidig wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Nieuw wachtwoord")]
        [Required(ErrorMessage = "Nieuw wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Herhaal wachtwoord")]
        [Required(ErrorMessage = "Herhaal wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool Success { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != ConfirmPassword)
            {
                //Check if the 2 new passwords are equal.
                yield return new ValidationResult("De opgegeven wachtwoorden moeten gelijk zijn.");
            }
        }
    }
}
