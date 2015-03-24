using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class AppointmentByStudentModel : AppointmentModel
    {
        [Display(Name = "Jouw SLB'er")]
        public CounselerModel Counseler { get; set; }
    }
}
