using System.ComponentModel.DataAnnotations;
namespace StudentFollowingSystem.ViewModels
{
    public class CounselerModel
    {
        public int Id { get; set; }

        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Display(Name = "Volledige naam")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
    }
}
