using System;

namespace StudentFollowingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}
