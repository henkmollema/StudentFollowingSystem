using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFollowingSystem.Controllers
{
    public class CounselingController : Controller
    {
        private readonly CounselingRepository _counselingRepository = new CounselingRepository();
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private readonly StudentRepository _studentRepository = new StudentRepository();

        public ActionResult Create(int appointmentId)
        {
            var model = new CounselingModel();
            var student = _studentRepository.GetById(_appointmentRepository.GetById(appointmentId).StudentId);
            var studentModel = Mapper.Map<StudentModel>(student);
            model.NextAppointment = DateTime.Now.AddDays(7);
            model.Status = student.Status;
            model.AppointmentId = appointmentId;
            model.StudentName = student.GetFullName();
            model.AppointmentDate = _appointmentRepository.GetById(appointmentId).DateTime;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CounselingModel model)
        {
            if (ModelState.IsValid)
            {
                // Map counseling view model to domain model.
                var counseling = Mapper.Map<Counseling>(model);
                var appointment = _appointmentRepository.GetById(model.AppointmentId);
                appointment.Noted = true;
                // Map student view model to domain model.
                var student = _studentRepository.GetById(_appointmentRepository.GetById(appointment.Id).StudentId);
                student.Status = model.Status;
                student.NextAppointment = model.NextAppointment;

                // todo: add counseling repository to class
                _counselingRepository.Add(counseling);
                _studentRepository.Update(student);
                _appointmentRepository.Update(appointment);

                // todo: redirect to details
                return RedirectToAction("Dashboard", "Counseler");
            }
        
            return View(model);
        }

        public ActionResult Details(int appointmentId)
        {
            var counseling = _counselingRepository.GetByAppointmentId(appointmentId);
            var model = Mapper.Map<CounselingModel>(counseling);
            var student = _studentRepository.GetById(_appointmentRepository.GetById(appointmentId).StudentId);
            model.StudentName = student.GetFullName();
            model.AppointmentDate = _appointmentRepository.GetById(appointmentId).DateTime;

            if (counseling == null)
            {
                return RedirectToAction("Create");
            }

            //PrepareStudentModel(model);
            return View(model);
        }

        public ActionResult Edit(int appointmentId)
        {
            var counseling = _counselingRepository.GetByAppointmentId(appointmentId);
            var model = Mapper.Map<CounselingModel>(counseling);
            var student = _studentRepository.GetById(_appointmentRepository.GetById(appointmentId).StudentId);
            model.StudentName = student.GetFullName();
            model.AppointmentDate = _appointmentRepository.GetById(appointmentId).DateTime;
            model.NextAppointment = student.NextAppointment;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CounselingModel model)
        {
            if (ModelState.IsValid)
            {
                // Map counseling view model to domain model.
                var counseling = _counselingRepository.GetByAppointmentId(model.AppointmentId);
                counseling.Comment = model.Comment;
                counseling.Private = model.Private;
                counseling.Status = model.Status;
                // Map student view model to domain model.
                var student = _studentRepository.GetById(_appointmentRepository.GetById(model.AppointmentId).StudentId);
                student.Status = model.Status;
                student.NextAppointment = model.NextAppointment;

                // todo: add counseling repository to class
                _counselingRepository.Update(counseling);
                _studentRepository.Update(student);

                // todo: redirect to details
                return RedirectToAction("Details", "Counseling", new { appointmentId = model.AppointmentId });
            }

            return View(model);
        }
    }
}