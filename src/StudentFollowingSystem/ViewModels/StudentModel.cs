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

        
        [Display(Name = "Student nr")]
        [Required(ErrorMessage = "Vul een Studentnummer in.")]
        public int? StudentNr { get; set; }


        [Display(Name = "Voornaam")]
        [Required(ErrorMessage = "Vul een voornaam in.")]
        public string FirstName { get; set; }

        
        [Display(Name = "Achternaam")]
        [Required(ErrorMessage = "Vul een achternaam in.")]
        public string LastName { get; set; }

        [Display(Name = "Volledige naam")]
        [Required(ErrorMessage = "Vul een volledige naam in.")]
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

        
        [Display(Name = "School email")]
        [Required(ErrorMessage = "Vul een e-mailadres in.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vul een geldig e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string SchoolEmail { get; set; }

        [Display(Name = "Klas")]
        [Required(ErrorMessage = "Vul een klas in.")]
        public int ClassId { get; set; }

        public ClassModel Class { get; set; }

        public IEnumerable<SelectListItem> ClassesList { get; set; }

        [Display(Name = "Telefoonnummer")]
        [Required(ErrorMessage = "Vul een geldig Telefoonnummer in")]
        public string Telephone { get; set; }

        [Display(Name = "Straatnaam")]
        [Required(ErrorMessage = "Vul een straatnaam in.")]
        public string StreetName { get; set; }

        [Display(Name = "Huisnummer")]
        [Required(ErrorMessage = "Vul een huisnummer in.")]
        public int? StreetNumber { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "Vul een postcode in.")]
        public string ZipCode { get; set; }

        [Display(Name = "Stad")]
        [Required(ErrorMessage = "Vul een woonplaats in.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht (")]
        [Display(Name = "Geboortedatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Vul een geldige datum in.")]

        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Inschrijvingsdatum is verplicht")]
        [Display(Name = "Inschrijvingsdatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Vul een geldige datum in.")]
        public DateTime? EnrollDate { get; set; }

        [Display(Name = "Vooropleiding")]
        [Required(ErrorMessage = "Vul een vooropleiding in.")]
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
