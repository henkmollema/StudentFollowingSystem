using System;
namespace StudentFollowingSystem.Models
{
    public class Counseling
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        public string Comment { get; set; }

        public bool Private { get; set; }
    }
}
