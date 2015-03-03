using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudentFollowingSystem.ViewModels
{
    public class ClassModel
    {
        public int Id { get; set; }

        [Display(Name = "Klas naam")]
        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }

        [Display(Name = "Afdeling")]
        public string Section { get; set; }

        [Display(Name = "SLB'er")]
        [Required(ErrorMessage = "SLB'er is verplicht")]
        public int CounselerId { get; set; }

        public CounselerModel Counseler { get; set; }

        public IEnumerable<SelectListItem> CounselerList { get; set; }
    }
}
