using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student nr")]
        public int StudentNr { get; set; }

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

        [Display(Name = "Klasse ID")]
        public int ClassId { get; set; }

        public IEnumerable<SelectListItem> ClassesList { get; set; }

        [Display(Name = "Telefoonnummer")]
        public string Telephone { get; set; }

        [Display(Name = "Straatnaam")]
        public string StreetName { get; set; }

        [Display(Name = "Huisnummer")]
        public int StreetNumber { get; set; }

        [Display(Name = "Postcode")]
        public string ZipCode { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Geboortedatum")]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Inschrijvingsdatum")]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "Vooropleiding")]
        public string PreStudy { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Extra info")]
        public string Details { get; set; }
    }
}
