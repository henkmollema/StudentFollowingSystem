using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StudentFollowingSystem.ViewModels
{
    public class AppointmentModel : IValidatableObject
    {
        //Create a new appointment.
        [Required(ErrorMessage = "Datum en tijd is verplicht")]
        [Display(Name = "Datum en tijd")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public string DateTimeString { get; set; }

        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Een locatie is verplicht")]
        [Display(Name = "Locatie")]
        public string Location { get; set; }

        public StudentModel Student { get; set; }

        public bool Accepted { get; set; }

        public bool Noted { get; set; }

        public int Id { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(DateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
            {
                DateTime = dateTime;
            }
            else
            {
                //Making sure a valid date is given.
                yield return new ValidationResult("Opgegeven datum is niet geldig");
            }

            if (DateTime < DateTime.Now)
            {
                //Making sure the date planned is in the future.
                yield return new ValidationResult("De datum/tijd moet in de toekomst liggen");
            }
        }
    }
}
