using System;
using System.Linq;
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
    public class StudentsController : ControllerBase
    {
        private readonly SubjectRepository _subjectRepository = new SubjectRepository();
        private readonly ClassRepository _classRepository = new ClassRepository();
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        [AuthorizeStudent]
        public ActionResult Dashboard()
        {
            var model = new StudentDashboardModel();
            var subjects = _subjectRepository.GetAll().Where(s => s.StartDate.Date == DateTime.Now.Date).ToList();
            model.Subjects = subjects;
            return View(model);
        }

        [AuthorizeCounseler]
        public ActionResult List()
        {
            var students = Mapper.Map<List<StudentModel>>(StudentRepository.GetAll());
            return View(students);
        }

        [AuthorizeCounseler]
        public ActionResult Create()
        {
            var model = new StudentModel { EnrollDate = new DateTime(DateTime.Now.Year - 1, 9, 1) };
            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler]
        [HttpPost]
        public ActionResult Create(StudentModel model)
        {
            if (ModelState.IsValid)
            {
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
                                      text = string.Format("Beste {1},{0}{0}Inlognaam: {3} {0}Wachtwoord: {2}{0}{0}",
                                          Environment.NewLine,
                                          student.FirstName,
                                          password,
                                          student.Email),
                                      subject = "Studenten Volg Systeem wachtwoord",
                                      to = new List<EmailAddress>
                                               {
                                                   new EmailAddress(student.Email, string.Format("{0} {1}", student.FirstName, student.LastName))
                                               },
                                  };
                    _mailEngine.Send(msg);

                    return RedirectToAction("List");
                }

                ModelState.AddModelError("", "Studentnummer komt al voor in de database.");
            }

            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler]
        public ActionResult Import()
        {
            return View();
        }

        [AuthorizeCounseler, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Import(string csv)
        {
            foreach (string[] d in csv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                      .Skip(1)
                                      .Select(line => line.Split(new[] { ';' })))
            {
                // Try to parse the student number.
                int studentNr;
                if (!int.TryParse(d[1], out studentNr))
                {
                    ModelState.AddModelError("", string.Format("StudentNr '{0}' is geen geldig nummer", d[1]));
                    break;
                }

                var existing = StudentRepository.GetByStudentNr(studentNr);
                if (existing != null)
                {
                    // Skip existing students.
                    continue;
                }

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

                string password = PasswordGenerator.CreateRandomPassword();
                student.Password = Crypto.HashPassword(password);

                // todo: mail

                StudentRepository.Add(student);
            }

            return View();
        }

        [AuthorizeCounseler]
        public ActionResult Details(int id)
        {
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return RedirectToAction("List");
            }

            var model = Mapper.Map<StudentModel>(student);
            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler]
        public ActionResult Edit(int id)
        {
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return RedirectToAction("List");
            }

            var model = Mapper.Map<StudentModel>(student);
            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeCounseler]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                var student = StudentRepository.GetById(model.Id);
                Mapper.Map(model, student);
                StudentRepository.Update(student);

                return RedirectToAction("List");
            }

            PrepareStudentModel(model);
            return View(model);
        }

        [AuthorizeStudent]
        public ActionResult Presence(int subjectId)
        {
            var subject = _subjectRepository.GetById(subjectId);
            if (subject == null)
            {
                return HttpNotFound();
            }

            DateTime now = DateTime.Now;
            if (subject.StartDate < now && now > subject.EndDate.AddMinutes(30))
            {
                // Redirect if current subject is not the current subject.
                return RedirectToAction("Dashboard");
            }

            var subjectModel = Mapper.Map<SubjectModel>(subject);

            var presenceModel = new PresenceModel
                                    {
                                        Subject = subjectModel
                                    };

            return View(presenceModel);
        }

        private void PrepareStudentModel(StudentModel model)
        {
            model.ClassesList = SelectList(_classRepository.GetAll(),
                c => c.Id,
                c => c.Name + (!string.IsNullOrEmpty(c.Section)
                                   ? string.Format(" ({0})", c.Section)
                                   : string.Empty));
        }
    }
}
