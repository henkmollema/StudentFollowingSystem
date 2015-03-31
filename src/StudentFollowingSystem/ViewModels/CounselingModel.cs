using StudentFollowingSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class CounselingModel
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        [Display(Name = "Commentaar")]
        [Required(ErrorMessage = "Vul commentaar in.")]
        public string Comment { get; set; }

        [Display(Name = "Privé")]
        public bool Private { get; set; }

        public Status Status { get; set; }

        [Required(ErrorMessage = "Een nieuwe datum is verplicht. (")]
        [Display(Name = "Volgende afspraak")]
        public DateTime? NextAppointment { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string StudentName { get; set; }
    }
}