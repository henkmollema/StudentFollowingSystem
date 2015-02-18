using System;
using System.Collections.Generic;
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
    [AuthorizeCounseler]
    public class StudentsController : Controller
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        public ActionResult Dashboard()
        {
            var model = new StudentDashboardModel();
            return View(model);
        }

        public ActionResult List()
        {
            var students = Mapper.Map<List<StudentModel>>(_studentRepository.GetAll());
            return View(students);
        }

        public ActionResult Add()
        {
            return View(new StudentModel());
        }

        [HttpPost]
        public ActionResult Add(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                var student = Mapper.Map<Student>(model);
                student.BirthDate = DateTime.Now.AddYears(-20);
                student.Status = Status.Green;
                student.ClassId = 1;
                student.StreetNumber = 1;
                student.EnrollDate = new DateTime(2014, 9, 1);

                string password = PasswordGenerator.CreateRandomPassword();
                student.Password = Crypto.HashPassword(password);
                _studentRepository.Add(student);

                var msg = new EmailMessage
                              {
                                  text = string.Format("Hoi {1},{0}{0} Inlognaam: {1} {0}Wachtwoord: {2}{0}{0}Groetjes.",
                                      Environment.NewLine,
                                      student.FirstName,
                                      password),
                                  subject = "SVS wachtwoord",
                                  to = new List<EmailAddress>
                                           {
                                               new EmailAddress(student.Email, string.Format("{0} {1}", student.FirstName, student.LastName))
                                           },

                              };
                _mailEngine.Send(msg);

                return RedirectToAction("List");
            }

            return View(model);
        }
    }
}
