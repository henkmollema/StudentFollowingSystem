using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Mails;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [RoutePrefix("afspraken")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private readonly ClassRepository _classRepository = new ClassRepository();
        private readonly MailHelper _mailHelper = new MailHelper();

        [AuthorizeCounseler, Route("overzicht")]
        public ActionResult List()
        {
            // Get all the appointments for the logged in counseler.
            var model = Mapper.Map<List<AppointmentModel>>(_appointmentRepository.GetAll().Where(a => a.CounselerId == Counseler.Id));
            return View(model);
        }

        [AuthorizeStudent, Route("afspraak-met-docent")]
        public ActionResult AppointmentByStudent()
        {
            DateTime tommorow = DateTime.Now.AddDays(1);
            var model = new AppointmentByStudentModel
                            {
                                // Default appointment date is tomorrow 10:00.
                                DateTimeString = new DateTime(tommorow.Year, tommorow.Month, tommorow.Day, 10, 0, 0).ToString("dd-MM-yyyy HH:mm")
                            };

            PrepareStudentCounselingRequestModel(model);

            return View(model);
        }

        [AuthorizeStudent, HttpPost, ValidateAntiForgeryToken, Route("afspraak-met-docent")]
        public ActionResult AppointmentByStudent(AppointmentByStudentModel model)
        {
            // A student wishes to make an appointment with their counseler.
            if (ModelState.IsValid)
            {
                // Get the logged in student and counseler of the class the student is in.
                var student = Student;
                var @class = _classRepository.GetById(student.ClassId);
                var counseler = CounselerRepository.GetById(@class.CounselerId);

                if (counseler != null && student != null)
                {
                    // Create the appointment for the student and couseler.
                    var appointment = new Appointment
                                          {
                                              CounselerId = counseler.Id,
                                              StudentId = student.Id,
                                              DateTime = model.DateTime,
                                              Location = model.Location
                                          };

                    int id = _appointmentRepository.Add(appointment);

                    // Send the appointment request mail.
                    var mail = new AppointmentMail
                                   {
                                       Counseler = counseler,
                                       Student = student,
                                       DateTime = appointment.DateTime,
                                       Location = appointment.Location,
                                       AcceptUrl = FullUrl("AppointmentResponse", new { id }),
                                       FromCounseler = false
                                   };

                    _mailHelper.SendAppointmentMail(mail);

                    return RedirectToAction("Details", "Appointment", new { id });
                }
            }

            PrepareStudentCounselingRequestModel(model);
            return View(model);
        }

        [AuthorizeCounseler, Route("afspraak-met-student/{studentId?}")]
        public ActionResult AppointmentByCounseler(int? studentId)
        {
            // A counseler wishes to create an appointment with their student.
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

        [AuthorizeCounseler, HttpPost, ValidateAntiForgeryToken, Route("afspraak-met-student/{studentId?}")]
        public ActionResult AppointmentByCounseler(AppointmentByCounselerModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the logged in counseler and the chosen student.
                var counseler = Counseler;
                var student = StudentRepository.GetById(model.StudentId);
                if (counseler != null && student != null)
                {
                    // Create the appointment for the counseler and student.
                    var appointment = new Appointment
                                          {
                                              CounselerId = counseler.Id,
                                              StudentId = model.StudentId,
                                              DateTime = model.DateTime,
                                              Location = model.Location
                                          };

                    int id = _appointmentRepository.Add(appointment);

                    // Send the appointment mail.
                    var mail = new AppointmentMail
                                   {
                                       Counseler = counseler,
                                       Student = student,
                                       DateTime = appointment.DateTime,
                                       Location = appointment.Location,
                                       AcceptUrl = FullUrl("AppointmentResponse", new { id }),
                                       FromCounseler = true
                                   };
                    _mailHelper.SendAppointmentMail(mail);

                    return RedirectToAction("Details", "Appointment", new { id });
                }
            }

            PrepareCounselerCounselingRequestModel(model);
            return View(model);
        }

        [Authorize, Route("details/{id}")]
        public ActionResult Details(int id)
        {
            // Get the appointment by its id.
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (!AuthorizeAppointment(appointment))
            {
                return new HttpUnauthorizedResult();
            }

            ViewBag.IsCounseler = Counseler != null;

            return View(appointment);
        }

        [Authorize, Route("afspraak-bevestigen/{id}")]
        public ActionResult AppointmentResponse(int id)
        {
            // Student or counseler wants to respond on the appointment request.
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (!AuthorizeAppointment(appointment))
            {
                return new HttpUnauthorizedResult();
            }

            var model = new AppointmentResponseModel
                            {
                                Appointment = appointment,
                                Accepted = true,
                                AlreadyAccepted = appointment.Accepted
                            };

            return View(model);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken, Route("afspraak-bevestigen/{id}")]
        public ActionResult AppointmentResponse(AppointmentResponseModel model)
        {
            var appointment = _appointmentRepository.GetById(model.Id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Update the appointment status.
                appointment.Accepted = model.Accepted;
                _appointmentRepository.Update(appointment);

                // Create a repsonse mail.
                var mail = new AppointmentRepsonseMail
                               {
                                   Appointment = appointment,
                                   Notes = model.Notes,
                                   FromCounseler = Counseler != null
                               };
                _mailHelper.SendAppointmentResponseMail(mail);

                return RedirectToDashboard();
            }

            model.Appointment = appointment;
            return View(model);
        }

        /// <summary>
        /// Checks that the specified <paramref name="appointment"/> belongs to the 
        /// logged in student or counseler.
        /// </summary>
        /// <param name="appointment">The appointment to authorize.</param>
        private bool AuthorizeAppointment(Appointment appointment)
        {
            var student = Student;
            var counseler = Counseler;
            return student != null && appointment.StudentId == student.Id ||
                   counseler != null && appointment.CounselerId == counseler.Id;
        }

        private void PrepareCounselerCounselingRequestModel(AppointmentByCounselerModel model)
        {
            var counseler = Counseler;

            // Populate the select list with all the students of the logged in counseler.
            model.StudentsList = SelectList(StudentRepository.GetAll().Where(s => s.Class.CounselerId == counseler.Id),
                s => s.Id,
                s => string.Format("{0} ({1})", s.GetFullName(), s.StudentNr));
        }

        private void PrepareStudentCounselingRequestModel(AppointmentByStudentModel model)
        {
            // Add the corresponding counseler to the view model.
            var student = Student;
            var @class = _classRepository.GetById(student.ClassId);
            var counseler = CounselerRepository.GetById(@class.CounselerId);

            model.Counseler = Mapper.Map<CounselerModel>(counseler);
        }
    }
}
