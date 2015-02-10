using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class StudentsController : Controller
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();

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
                student.Password = Crypto.HashPassword("test");
                _studentRepository.Add(student);

                return RedirectToAction("List");
            }

            return View(model);
        }
    }
}
