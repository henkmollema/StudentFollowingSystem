using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class AppointmentResponseModel : IValidatableObject
    {
        public int Id { get; set; }

        public Appointment Appointment { get; set; }

        [Display(Name = "Akkoord")]
        public bool Accepted { get; set; }

        [Display(Name = "Opmerkingen")]
        public string Notes { get; set; }

        public bool AlreadyAccepted { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Accepted && string.IsNullOrEmpty(Notes))
            {
                yield return new ValidationResult("Een reden is verplicht als je de afspraak niet accepteerdt");
            }
        }
    }
}
