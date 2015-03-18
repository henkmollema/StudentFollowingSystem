using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StudentFollowingSystem.ViewModels
{
    public class AppointmentModel : IValidatableObject
    {
        [Required(ErrorMessage = "Datum en tijd is verplicht")]
        [Display(Name = "Datum en tijd")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public string DateTimeString { get; set; }

        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Een locatie is verplicht")]
        [Display(Name = "Locatie")]
        public string Location { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(DateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
            {
                DateTime = dateTime;
            }
            else
            {
                yield return new ValidationResult("Opgegeven datum is niet geldig");
            }

            if (DateTime < DateTime.Now)
            {
                yield return new ValidationResult("De datum/tijd moet in de toekomst liggen");
            }
        }
    }
}
