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
            if (ModelState.IsValid)
            {
                var student = Student;
                var @class = _classRepository.GetById(student.ClassId);
                var counseler = CounselerRepository.GetById(@class.CounselerId);

                if (counseler != null && student != null)
                {
                    var appointment = new Appointment
                                          {
                                              CounselerId = counseler.Id,
                                              StudentId = student.Id,
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
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (!AuthorizeAppointment(appointment))
            {
                return new HttpUnauthorizedResult();
            }

            return View(appointment);
        }

        private bool AuthorizeAppointment(Appointment appointment)
        {
            var student = Student;
            var counseler = Counseler;
            if (student != null && appointment.StudentId != student.Id ||
                counseler != null && appointment.CounselerId != counseler.Id)
            {
                return false;
            }

            return true;
        }

        [Authorize, Route("afspraak-bevestigen/{id}")]
        public ActionResult AppointmentResponse(int id)
        {
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
                appointment.Accepted = model.Accepted;
                _appointmentRepository.Update(appointment);

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

        private void PrepareCounselerCounselingRequestModel(AppointmentByCounselerModel model)
        {
            // Populate the select list with all the students.
            model.StudentsList = SelectList(StudentRepository.GetAll(),
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
