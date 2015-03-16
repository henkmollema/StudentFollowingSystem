using System;
using System.Linq;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class CounselerController : ControllerBase
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();
        private readonly StudentRepository _studentRepository = new StudentRepository();
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();

        public ActionResult Dashboard()
        {
            Counseler counseler = Counseler;
            var appointments = _appointmentRepository.GetAppointmentsByCounseler(counseler.Id, GetFirstSaturday(), DateTime.Now);
            var students = _studentRepository.GetAll();
            foreach (var appointment in appointments)
            {
                appointment.Counseler = counseler;
            }

            var model = new CounselerDashboardModel
                            {
                                Appointments = appointments,
                                Students = students.Where(s => s.Status == Status.Orange ||
                                                               s.Status == Status.Red ||
                                                               DateTime.Now > s.LastAppointment.AddMonths(3))
                                                   .OrderByDescending(s => s.Status)
                                                   .ThenBy(s => s.LastAppointment)
                                                   .ToList()
                            };

            return View(model);
        }

        public ActionResult CounselingRequest(int? studentId)
        {
            DateTime tommorow = DateTime.Now.AddDays(1);
            var model = new CounselerCounselingRequestModel
                            {
                                DateTimeString = new DateTime(tommorow.Year, tommorow.Month, tommorow.Day, 10, 0, 0).ToString("dd-MM-yyyy HH:mm")
                            };

            PrepareCounselerCounselingRequestModel(model);

            if (studentId.HasValue)
            {
                model.StudentId = studentId.Value;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CounselingRequest(CounselerCounselingRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var counseler = Counseler;
                var appointment = new Appointment
                                      {
                                          CounselerId = counseler.Id,
                                          StudentId = model.StudentId,
                                          DateTime = model.DateTime,
                                          Location = model.Location
                                      };

                _appointmentRepository.Add(appointment);

                // todo: send mail.

                // todo: redirect to appointment overview.
                return RedirectToAction("Dashboard");
            }

            PrepareCounselerCounselingRequestModel(model);
            return View(model);
        }

        private void PrepareCounselerCounselingRequestModel(CounselerCounselingRequestModel model)
        {
            model.StudentsList = _studentRepository.GetAll()
                                                   .Select(s => new SelectListItem
                                                                    {
                                                                        Value = s.Id.ToString(),
                                                                        Text = string.Format("{0} ({1})", s.GetFullName(), s.StudentNr)
                                                                    });
        }

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
