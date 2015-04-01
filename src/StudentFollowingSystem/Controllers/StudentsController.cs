using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using Mandrill;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.Services;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [RoutePrefix("studenten")]
    public class StudentsController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private readonly SubjectRepository _subjectRepository = new SubjectRepository();
        private readonly PresenceRepository _presenceRepository = new PresenceRepository();
        private readonly ClassRepository _classRepository = new ClassRepository();
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        [AuthorizeStudent, Route("dashboard")]
        public ActionResult Dashboard()
        {
            // Create the dashboard for a student with their subjects, presences and appointments.
            var student = Student;
            var model = new StudentDashboardModel
                            {
                                Subjects = _subjectRepository.GetAll()
                                                             .Where(s => s.StartDate.Date == DateTime.Now.Date)
                                                             .ToList(),
                                Presences = _presenceRepository.GetByStudent(student.Id),
                                Appointments = _appointmentRepository.GetAppointmentsByStudent(student.Id, GetFirstSaturday(), DateTime.Now)
                            };


            return View(model);
        }

        [AuthorizeCounseler, Route("overzicht")]
        public ActionResult List()
        {
            // Show all the students of the currently logged in counseler.
            var students = Mapper.Map<List<StudentModel>>(StudentRepository.GetAll().Where(s => s.Class.CounselerId == Counseler.Id));
            return View(students);
        }

        [AuthorizeCounseler, Route("nieuw")]
        public ActionResult Create()
        {
            // Prepare the student view model.
            var model = new StudentModel
                            {
                                // Default enroll date is September of last year.
                                EnrollDate = new DateTime(DateTime.Now.Year - 1, 9, 1)
                            };

            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler, HttpPost, Route("nieuw")]
        public ActionResult Create(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate students.
                var duplicate = StudentRepository.GetByStudentNr(model.StudentNr.GetValueOrDefault());
                if (duplicate == null)
                {
                    // Map the  student view model to the domain model.
                    var student = Mapper.Map<Student>(model);
                    student.Active = true;

                    // Generate a password for the student and hash it.
                    string password = PasswordGenerator.CreateRandomPassword();
                    student.Password = Crypto.HashPassword(password);
                    StudentRepository.Add(student);

                    // Create a mail message with the password and mail it to the student.
                    var msg = new EmailMessage
                                  {
                                      text = string.Format("Beste {1},{0}{0}NHL Hogeschool maakt gebruikt van het Studenten Volg Systeem.{0}Hier kunnen studenten en SLB'ers de studie voortgang bekijken en bijhouden. {0}De NHL heeft een acount voor je aangenaakt. Dit zijn de gegevens: {0}{0}Inlognaam: {3} {0}Wachtwoord: {2}{0}{0}Met vriendelijke groet,{0}{0}NHL Hogeschool",
                                          Environment.NewLine,
                                          student.FirstName,
                                          password,
                                          student.Email),
                                      subject = "Studenten Volg Systeem account",
                                      to = new List<EmailAddress>
                                               {
                                                   new EmailAddress(student.Email, string.Format("{0} {1}", student.FirstName, student.LastName)),
                                                   new EmailAddress(student.SchoolEmail, string.Format("{0} {1}", student.FirstName, student.LastName), "cc")
                                               },
                                  };
                    _mailEngine.Send(msg);

                    return RedirectToAction("List");
                }

                // Student is a duplicate.
                ModelState.AddModelError("", "Studentnummer komt al voor in de database.");
            }

            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler, Route("importeren")]
        public ActionResult Import()
        {
            return View();
        }

        [AuthorizeCounseler, HttpPost, ValidateAntiForgeryToken, Route("importeren")]
        public ActionResult Import(string csv)
        {
            // A csv is supplied, try to parse it.
            foreach (string[] d in csv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                      .Skip(1)
                                      .Select(line => line.Split(new[] { ';' })))
            {
                // Try to parse the student number.
                int studentNr;
                if (!int.TryParse(d[1], out studentNr))
                {
                    ModelState.AddModelError("", string.Format("StudentNr '{0}' is geen geldig nummer", d[1]));
                    continue;
                }

                var existing = StudentRepository.GetByStudentNr(studentNr);
                if (existing != null)
                {
                    // Skip existing students.
                    continue;
                }

                // Create a new student from the csv data.
                var student = new Student
                                  {
                                      StudentNr = studentNr,
                                      FirstName = d[2],
                                      LastName = d[4],
                                      SchoolEmail = d[5].ToLower(),
                                      Email = d[6].ToLower(),
                                      ClassId = int.Parse(d[7]),
                                      Telephone = d[8],
                                      StreetNumber = int.Parse(d[9]),
                                      StreetName = d[10],
                                      ZipCode = d[11],
                                      City = d[12],
                                      PreStudy = d[13],
                                      Status = (Status)Enum.Parse(typeof (Status), d[14]),
                                      Active = d[15] == "1",
                                      EnrollDate = DateTime.Parse(d[16]),
                                      LastAppointment = DateTime.Parse(d[16]),
                                      BirthDate = DateTime.Parse(d[17])
                                  };

                // Generate a password.
                string password = PasswordGenerator.CreateRandomPassword();
                student.Password = Crypto.HashPassword(password);

                // todo: mail

                StudentRepository.Add(student);
            }

            return View();
        }

        [AuthorizeCounseler, Route("details/{id}")]
        public ActionResult Details(int id)
        {
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return RedirectToAction("List");
            }

            // Map the student to a view model.
            var model = Mapper.Map<StudentModel>(student);
            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler, Route("wijzigen/{id}")]
        public ActionResult Edit(int id)
        {
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return RedirectToAction("List");
            }

            // Map the student to a view model.
            var model = Mapper.Map<StudentModel>(student);
            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler, HttpPost, ValidateAntiForgeryToken, Route("wijzigen/{id}")]
        public ActionResult Edit(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                var student = StudentRepository.GetById(model.Id);
                if (student == null)
                {
                    return HttpNotFound();
                }

                // Map the new values from the view model to the existing student.
                Mapper.Map(model, student);
                StudentRepository.Update(student);

                return RedirectToAction("List");
            }

            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeStudent, Route("gegevens-wijzigen")]
        public ActionResult EditInfo()
        {
            // Get the currently logged in student.
            var student = Student;
            if (student == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Map the student to a view model.
            var model = Mapper.Map<StudentInfoModel>(student);
            return View(model);
        }

        [AuthorizeStudent, HttpPost, ValidateAntiForgeryToken, Route("gegevens-wijzigen")]
        public ActionResult EditInfo(StudentInfoModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged in student.
                var student = Student;

                // Map the new values from the view model to the current student.
                Mapper.Map(model, student);

                // Update the student in the database.
                StudentRepository.Update(student);

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        [AuthorizeStudent, Route("aanwezigheid/{subjectId}")]
        public ActionResult Presence(int subjectId)
        {
            // Get the specified subject.
            var subject = _subjectRepository.GetById(subjectId);
            if (subject == null)
            {
                return HttpNotFound();
            }

            // Get the current presence.
            int studentId = Student.Id;
            var presence = _presenceRepository.GetByStudent(studentId);

            // Map the subject to a view model.
            var subjectModel = Mapper.Map<SubjectModel>(subject);

            // Create the presence view model.
            var presenceModel = new PresenceModel
                                    {
                                        Subject = subjectModel,
                                        // Check if student is already present.
                                        AlreadyPresent = presence.Any(p => p.IsPresent(subjectId, studentId)),
                                        PastSubject = subject.IsPastSubject()
                                    };

            return View(presenceModel);
        }

        [AuthorizeStudent, HttpPost, ValidateAntiForgeryToken, Route("aanwezigheid/{subjectId}")]
        public ActionResult Presence(int subjectId, FormCollection form)
        {
            // Get the specified subject.
            var subject = _subjectRepository.GetById(subjectId);
            if (subject == null)
            {
                return HttpNotFound();
            }

            // Validate that the subject is currently going on.
            if (subject.IsCurrentSubject())
            {
                // Get the current presence of a student.
                int studentId = Student.Id;
                var presence = _presenceRepository.GetByStudent(studentId);

                // Check if student is not already present.
                if (presence.All(p => p.StudentId != studentId))
                {
                    _presenceRepository.Add(new Presence { StudentId = Student.Id, SubjectId = subjectId });
                }
            }

            return RedirectToAction("Dashboard");
        }

        private void PrepareStudentModel(StudentModel model)
        {
            // Create a select list of classes.
            model.ClassesList = SelectList(_classRepository.GetAll(),
                c => c.Id,
                c => c.Name + (!string.IsNullOrEmpty(c.Section)
                                   ? string.Format(" ({0})", c.Section)
                                   : string.Empty));
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
