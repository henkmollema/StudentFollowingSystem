using System.Web.Mvc;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.ViewModels;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;
using System;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class CounselerController : Controller
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();

        public ActionResult Dashboard()
        {
            Counseler counseler = _counselerRepository.GetByEmail(User.Identity.Name);
            var appointments = _appointmentRepository.GetAppointmentsByCounseler(counseler.Id, GetFirstSaturday(), DateTime.Now);
            foreach (var appointment in appointments)
            {
                appointment.Counseler = counseler;
            }

            var model = new CounselerDashboardModel();
            model.Appointments = appointments;
            return View(model);
        }

        private DateTime GetFirstSaturday()
        {
            DateTime date = DateTime.Now;
            int days = 0;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    days = 5;
                    break;
                case DayOfWeek.Tuesday:
                    days = 4;
                    break;
                case DayOfWeek.Wednesday:
                    days = 3;
                    break;
                case DayOfWeek.Thursday:
                    days = 2;
                    break;
                case DayOfWeek.Friday:
                    days = 1;
                    break;
                case DayOfWeek.Saturday:
                    days = 7;
                    break;
                case DayOfWeek.Sunday:
                    days = 6;
                    break;
            }
            
            return date.AddDays(days);
        }
    }
}
