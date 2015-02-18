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
    public class StudentsController : Controller
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();
        
        [AuthorizeStudent]
        public ActionResult Dashboard()
        {
            var model = new StudentDashboardModel();
            return View(model);
        }

        [AuthorizeCounseler]
        public ActionResult List()
        {
            var students = Mapper.Map<List<StudentModel>>(_studentRepository.GetAll());
            return View(students);
        }

        [AuthorizeCounseler]
        public ActionResult Add()
        {
            return View(new StudentModel());
        }

        [AuthorizeCounseler, HttpPost]
        public ActionResult Add(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the  student view model to the domain model.
                var student = Mapper.Map<Student>(model);
                
                // Generate a password for the student and hash it.
                string password = PasswordGenerator.CreateRandomPassword();
                student.Password = Crypto.HashPassword(password);
                _studentRepository.Add(student);

                // Create a mail message with the password and mail it to the student.
                var msg = new EmailMessage
                              {
                                  text = string.Format("Hoi {1},{0}{0} Inlognaam: {3} {0}Wachtwoord: {2}{0}{0}Groetjes.",
                                      Environment.NewLine,
                                      student.FirstName,
                                      password,
                                      student.Email),
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
