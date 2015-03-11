using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class StudentModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student nr")]
        public int? StudentNr { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
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

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "School email")]
        public string SchoolEmail { get; set; }

        [Display(Name = "Klas")]
        public int ClassId { get; set; }

        public ClassModel Class { get; set; }

        public IEnumerable<SelectListItem> ClassesList { get; set; }

        [Display(Name = "Telefoonnummer")]
        public string Telephone { get; set; }

        [Display(Name = "Straatnaam")]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "Huisnummer")]
        public int? StreetNumber { get; set; }

        [Display(Name = "Postcode")]
        public string ZipCode { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Geboortedatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Inschrijvingsdatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EnrollDate { get; set; }

        [Display(Name = "Vooropleiding")]
        public string PreStudy { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Actief")]
        public bool Active { get; set; }

        [Display(Name = "Extra info")]
        public string Details { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate.GetValueOrDefault().Year + 16 > DateTime.Now.Year)
            {
                yield return new ValidationResult("De geboortedatum van de student klopt niet. Een student moet minimaal 16 jaar zijn.");
            }

            if (BirthDate > EnrollDate)
            {
                yield return new ValidationResult("De inschrijfdatum kan niet voor de geboortedatum liggen.");
            }
        }
    }
}
