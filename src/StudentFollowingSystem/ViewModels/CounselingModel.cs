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

        public string Comment { get; set; }

        public bool Private { get; set; }

        public Status Status { get; set; }

        public DateTime? NextAppointment { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string StudentName { get; set; }
    }
}