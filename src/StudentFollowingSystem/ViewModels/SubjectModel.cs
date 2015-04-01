using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace StudentFollowingSystem.ViewModels
{
    public class SubjectModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Vak naam")]
        [Required(ErrorMessage = "Geef hier de naam van het vak aan.")]
        public string Name { get; set; }

        [Display(Name = "Locatie")]
        [Required(ErrorMessage = "Locatie van de les.")]
        public string Locatie { get; set; }

        [Display(Name = "Klas")]
        [Required(ErrorMessage = "Vul een klas in.")]
        public int ClassId { get; set; }

        public ClassModel Class { get; set; }

        public IEnumerable<SelectListItem> ClassesList { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Start datum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public string StartDateString { get; set; }


        [Display(Name = "Start datum")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Eind datum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public string EndDateString { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime startDate;
            if (DateTime.TryParseExact(StartDateString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out startDate))
            {
                StartDate = startDate;
            }
            else
            {
                yield return new ValidationResult("Opgegeven start datum/tijd is niet geldig");
            }

            DateTime endDate;
            if (DateTime.TryParseExact(EndDateString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out endDate))
            {
                EndDate = endDate;
            }
            else
            {
                yield return new ValidationResult("Opgegeven eind datum/tijd is niet geldig");
            }

            if (StartDate > EndDate)
            {
                // Start date is after end date.
                yield return new ValidationResult("De start datum/tijd kan niet na de eind datum/tijd liggen.");
            }
        }
    }
}
