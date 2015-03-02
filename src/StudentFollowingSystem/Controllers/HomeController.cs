using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFollowingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Menu()
        {
            var model = new MenuModel();
            if (Request.IsAuthenticated && User != null && User.Identity.IsAuthenticated)
            {
                var student = _studentRepository.GetByEmail(User.Identity.Name);
                if (student != null)
                {
                    model.IsStudent = true;
                    model.Username = student.FullName;
                }
                else
                {
                    var counseler = _counselerRepository.GetByEmail(User.Identity.Name);
                    if (counseler != null)
                    {
                        model.IsCounseler = true;
                        model.Username = counseler.FullName;
                    }
                }
            }
            
            return PartialView("_Menu", model);
        }
    }
}