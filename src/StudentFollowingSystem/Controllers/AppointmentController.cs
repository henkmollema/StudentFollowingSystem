using System;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Mails;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private readonly MailHelper _mailHelper = new MailHelper();

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
                // Prefill the student id from the querystring.
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
                var student = StudentRepository.GetById(model.StudentId);
                if (counseler != null && student != null)
                {
                    var appointment = new Appointment
                                          {
                                              CounselerId = counseler.Id,
                                              StudentId = model.StudentId,
                                              DateTime = model.DateTime,
                                              Location = model.Location
                                          };

                    int id = _appointmentRepository.Add(appointment);

                    var mail = new AppointmentMail
                                   {
                                       Counseler = counseler,
                                       Student = student,
                                       DateTime = appointment.DateTime,
                                       Location = appointment.Location,
                                       AcceptUrl = Url.Action("AnswerAppointmentRequest", new { id })
                                   };
                    _mailHelper.SendAppointmentByCounseler(mail);

                    return RedirectToAction("Details", "Appointment", new { id });
                }
            }

            PrepareCounselerCounselingRequestModel(model);
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return Content("TODO. AppointmentId: " + id);
            return View();
        }

        public ActionResult AnswerAppointmentRequest(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            appointment.Accepted = true;
            _appointmentRepository.Update(appointment);

            return Content("Accepted appointment " + id);
            //return View();
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
