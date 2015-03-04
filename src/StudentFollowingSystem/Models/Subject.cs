using System;
using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.Models
{
    public class Subject
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Onderwerp")]
        public string Name { get; set; }

        [Display(Name = "Begin les")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Einde les")]
        public DateTime EndDate { get; set; }
    }
}
