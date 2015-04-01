using System;
using System.Linq;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler, RoutePrefix("docent")]
    public class CounselerController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();

        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            // Get the logged in counseler.
            var counseler = Counseler;

            // Get the appointments for a counseler.
            var appointments = _appointmentRepository.GetAppointmentsByCounseler(counseler.Id, GetFirstSaturday(), DateTime.Now);

            // Get the students of a counseler.
            var students = StudentRepository.GetAll().Where(s => s.Class.CounselerId == counseler.Id);

            // Create the dashboard view model.
            var model = new CounselerDashboardModel
                            {
                                Appointments = appointments.Where(a => !a.Noted).ToList(),
                                Students = students.Where(s => s.Status == Status.Orange ||
                                                               s.Status == Status.Red ||
                                                               DateTime.Now >= s.NextAppointment)
                                                   .OrderByDescending(s => s.Status)
                                                   .ThenBy(s => s.NextAppointment)
                                                   .ToList()
                            };

            return View(model);
        }

        /// <summary>
        /// Gets the first saturday from today.
        /// </summary>
        private static DateTime GetFirstSaturday()
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
