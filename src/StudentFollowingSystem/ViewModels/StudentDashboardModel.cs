using System.Collections.Generic;
using System.Linq;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class StudentDashboardModel
    {
        public StudentDashboardModel()
        {
            Subjects = new List<Subject>();
        }

        public List<Appointment> Appointments { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<Presence> Presences { get; set; }

        public bool IsPresent(Subject subject)
        {
            return Presences.Any(p => p.SubjectId == subject.Id);
        }
    }
}
