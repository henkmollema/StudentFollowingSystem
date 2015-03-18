using System;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();

        public ActionResult AppointmentByCounseler(int? studentId)
        {
            DateTime tommorow = DateTime.Now.AddDays(1);
            var model = new AppointmentByCounselerModel
                            {
                                // Default appointment date is tomorrow 10:00.
                                DateTimeString = new DateTime(tommorow.Year, tommorow.Month, tommorow.Day, 10, 0, 0).ToString("dd-MM-yyyy HH:mm")
                            };

            PrepareCounselerCounselingRequestModel(model);

            if (studentId.HasValue)
            {
                model.StudentId = studentId.Value;
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AppointmentByCounseler(AppointmentByCounselerModel model)
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
                return RedirectToAction("Dashboard", "Counseler");
            }

            PrepareCounselerCounselingRequestModel(model);
            return View(model);
        }

        private void PrepareCounselerCounselingRequestModel(AppointmentByCounselerModel model)
        {
            // Populate the select list with all the students.
            model.StudentsList = SelectList(StudentRepository.GetAll(),
                s => s.Id,
                s => string.Format("{0} ({1})", s.GetFullName(), s.StudentNr));
        }
    }
}
