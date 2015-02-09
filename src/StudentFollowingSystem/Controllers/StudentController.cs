using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;
using Validatr.Filters;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeStudent]
    public class StudentController : Controller
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();

        public ActionResult Dashboard()
        {
            var model = new StudentDashboardModel();
            return View(model);
        }

        public ActionResult List()
        {
            var students = new List<StudentModel>();
            return View(students);
        }

        public ActionResult Add()
        {
            return View(new StudentModel());
        }

        [HttpPost, Validate]
        public ActionResult Add(StudentModel model)
        {
            var student = Mapper.Map<Student>(model);
            _studentRepository.Add(student);

            return View(new StudentModel());
        }
    }
}
