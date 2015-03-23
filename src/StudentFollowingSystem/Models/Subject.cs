using System;

namespace StudentFollowingSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Locatie { get; set; }

        public int ClassId { get; set; }

        public bool IsPastSubject()
        {
            DateTime now = DateTime.Now;
            return StartDate < now && now > EndDate.AddMinutes(30);
        }
    }
}
