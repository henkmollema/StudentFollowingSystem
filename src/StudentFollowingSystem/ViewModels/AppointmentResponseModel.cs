using System.ComponentModel.DataAnnotations;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class AppointmentResponseModel
    {
        public int Id { get; set; }

        public Appointment Appointment { get; set; }

        [Display(Name = "Akkoord")]
        public bool Accepted { get; set; }

        [Display(Name = "Opmerkingen")]
        public string Notes { get; set; }

        public bool AlreadyAccepted { get; set; }
    }
}
