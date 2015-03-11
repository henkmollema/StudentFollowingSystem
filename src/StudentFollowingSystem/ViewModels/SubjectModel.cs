using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentFollowingSystem.ViewModels
{
    public class SubjectModel
    {
        public int Id { get; set; }

        [Display(Name = "Vak naam")]
        public string Name { get; set; }

        [Display(Name = "Start datum")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Eind datum")]
        public DateTime EndDate { get; set; }
    }
}