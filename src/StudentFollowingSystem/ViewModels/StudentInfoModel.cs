using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class StudentInfoModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vul een e-mailadres in.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vul een geldig e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; }

        [Display(Name = "Telefoonnummer")]
        [Required(ErrorMessage = "Vul een geldig Telefoonnummer in")]
        public string Telephone { get; set; }
    }
}
