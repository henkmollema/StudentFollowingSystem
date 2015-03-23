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
            model.Student = studentModel;
            model.Student.NextAppointment = DateTime.Now.AddDays(7);

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
                counseling.StudentId = appointment.StudentId;
                counseling.CounselerId = appointment.CounselerId;
                // Map student view model to domain model.
                var student = _studentRepository.GetById(counseling.StudentId);
                student.Status = counseling.Student.Status;
                student.NextAppointment = counseling.Student.NextAppointment;

                // todo: add counseling repository to class
                _counselingRepository.Add(counseling);
                _studentRepository.Add(student);

                // todo: redirect to details
                return RedirectToAction("Dashboard", "Counseler");
            }
        
            return View(model);
        }
    }
}