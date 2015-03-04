using StudentFollowingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentFollowingSystem.ViewModels
{
    public class CounselerDashboardModel
    {
        public List<Appointment> Appointments { get; set; }

        public List<Student> Students { get; set; }
    }
}