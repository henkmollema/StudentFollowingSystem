using StudentFollowingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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