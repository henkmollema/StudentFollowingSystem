using System;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Mails;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;
using AutoMapper;
using System.Collections.Generic;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
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
                                       AcceptUrl = FullUrl("AppointmentResponse", new { id })
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
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }

        public ActionResult AppointmentResponse(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            var model = new AppointmentResponseModel
                            {
                                Appointment = appointment,
                                Accepted = true,
                                AlreadyAccepted = appointment.Accepted
                            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
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
                                   FromCounseler = true // todo...
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

        [AuthorizeCounseler]
        public ActionResult List()
        {
            var model = Mapper.Map<List<AppointmentModel>>(_appointmentRepository.GetAll());
            return View(model);
        }
    }
}
