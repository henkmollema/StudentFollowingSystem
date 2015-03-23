using System.Collections.Generic;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class StudentDashboardModel
    {
        public StudentDashboardModel()
        {
            Subjects = new List<Subject>();
        }

        public List<Subject> Subjects { get; set; }
    }
}
