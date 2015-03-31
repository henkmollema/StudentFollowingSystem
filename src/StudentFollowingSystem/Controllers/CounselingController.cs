using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [RoutePrefix("slb-gesprek")]
    public class CounselingController : Controller
    {
        private readonly CounselingRepository _counselingRepository = new CounselingRepository();
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private readonly StudentRepository _studentRepository = new StudentRepository();

        [Route("nieuw/{appointmentId}")]
        public ActionResult Create(int appointmentId)
        {
            var appointment = _appointmentRepository.GetById(appointmentId);
            var student = _studentRepository.GetById(appointment.StudentId);

            var model = new CounselingModel
                            {
                                NextAppointment = appointment.DateTime.AddMonths(3),
                                Status = student.Status,
                                AppointmentId = appointmentId,
                                StudentName = student.GetFullName(),
                                AppointmentDate = appointment.DateTime
                            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("nieuw/{appointmentId}")]
        public ActionResult Create(CounselingModel model)
        {
            if (ModelState.IsValid)
            {
                // Set the appointment to 'noted'.
                var appointment = _appointmentRepository.GetById(model.AppointmentId);
                appointment.Noted = true;
                _appointmentRepository.Update(appointment);

                // Map the view model to a domain model.
                var counseling = Mapper.Map<Counseling>(model);
                _counselingRepository.Add(counseling);

                // Update student data based on the appointment.
                var student = _studentRepository.GetById(appointment.StudentId);
                student.Status = model.Status;
                student.NextAppointment = model.NextAppointment;
                student.LastAppointment = appointment.DateTime;

                _studentRepository.Update(student);
                return RedirectToAction("Details", new { appointmentId = appointment.Id });
            }

            return View(model);
        }

        [Route("details/{appointmentId}")]
        public ActionResult Details(int appointmentId)
        {
            // Get the counseling by the appointment id and map it to a view model.
            var counseling = _counselingRepository.GetByAppointment(appointmentId);
            if (counseling == null)
            {
                return RedirectToAction("Create");
            }

            var model = Mapper.Map<CounselingModel>(counseling);

            // Add the data of the corresponding student to the view model.
            var appointment = _appointmentRepository.GetById(appointmentId);
            var student = _studentRepository.GetById(appointment.StudentId);
            model.StudentName = student.GetFullName();
            model.AppointmentDate = appointment.DateTime;

            return View(model);
        }

        [Route("wijzigen/{appointmentId}")]
        public ActionResult Edit(int appointmentId)
        {
            // Get the counseling by the appointment id and map it to a view model.
            var counseling = _counselingRepository.GetByAppointment(appointmentId);
            if (counseling == null)
            {
                return RedirectToAction("Create");
            }

            var model = Mapper.Map<CounselingModel>(counseling);

            // Add the data of the corresponding student to the view model.
            var appointment = _appointmentRepository.GetById(appointmentId);
            var student = _studentRepository.GetById(appointment.StudentId);
            model.StudentName = student.GetFullName();
            model.AppointmentDate = appointment.DateTime;
            model.NextAppointment = student.NextAppointment;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("wijzigen/{appointmentId}")]
        public ActionResult Edit(CounselingModel model)
        {
            if (ModelState.IsValid)
            {
                var counseling = _counselingRepository.GetByAppointment(model.AppointmentId);
                counseling.Comment = model.Comment;
                counseling.Private = model.Private;
                counseling.Status = model.Status;

                var student = _studentRepository.GetById(_appointmentRepository.GetById(model.AppointmentId).StudentId);
                student.Status = model.Status;
                student.NextAppointment = model.NextAppointment;

                _counselingRepository.Update(counseling);
                _studentRepository.Update(student);

                return RedirectToAction("Details", new { appointmentId = model.AppointmentId });
            }

            return View(model);
        }
    }
}
