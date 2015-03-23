using StudentFollowingSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class CounselingModel
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public StudentModel Student { get; set; }

        public int CounselerId { get; set; }

        public Counseler Counseler { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        public string Comment { get; set; }

        public bool Private { get; set; }

        public DateTime LastAppointment { get; set; }

        public DateTime? NextAppointment { get; set; }
    }
}