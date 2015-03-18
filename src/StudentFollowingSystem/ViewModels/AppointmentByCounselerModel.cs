using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudentFollowingSystem.ViewModels
{
    public class AppointmentByCounselerModel : AppointmentModel
    {
        [Required(ErrorMessage = "Kies een student")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        public IEnumerable<SelectListItem> StudentsList { get; set; }
    }
}
