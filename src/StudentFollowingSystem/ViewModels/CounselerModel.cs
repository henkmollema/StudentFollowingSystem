using System.ComponentModel.DataAnnotations;
namespace StudentFollowingSystem.ViewModels
{
    public class CounselerModel
    {
        public int Id { get; set; }

        [Display(Name = "Voornaam")]
        [Required(ErrorMessage = "Vul uw voornaam in.")]
        public string FirstName { get; set; }

        [Display(Name = "Achternaam")]
        [Required(ErrorMessage = "Vul uw achternaam in.")]
        public string LastName { get; set; }

        [Display(Name = "Volledige naam")]
        [Required(ErrorMessage = "Vul uw volledige naam in.")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Display(Name = "Email")]
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
