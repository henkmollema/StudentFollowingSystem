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

        /// <summary>
        /// Gets a value indicating whether the current subject is ongoing at the moment.
        /// </summary>
        public bool IsCurrentSubject()
        {
            DateTime now = DateTime.Now;
            return StartDate < now && now < EndDate.AddMinutes(30);
        }

        /// <summary>
        /// Gets a value indicating whether the current subject is in the past.
        /// </summary>
        public bool IsPastSubject()
        {
            return DateTime.Now > EndDate.AddMinutes(30);
        }
    }
}
