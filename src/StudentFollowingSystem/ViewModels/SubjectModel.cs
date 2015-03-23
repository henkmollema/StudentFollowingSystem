using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StudentFollowingSystem.Models;
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

        public IEnumerable<SelectListItem> ClassesList { get; set; }

        [Display(Name = "Start datum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Vul een geldige datum in: (dd-mm-yyyy)")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Eind datum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Vul een geldige datum in: (dd-mm-yyyy)")]
        public DateTime? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                // Start date is after end date.
                yield return new ValidationResult("De start datum/tijd kan niet na de eind datum/tijd liggen.");
            }
        }
    }
}