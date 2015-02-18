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
                model.IsStudent = student != null;

                var counseler = _counselerRepository.GetByEmail(User.Identity.Name);
                model.IsCounseler = counseler != null;
            }
            
            return PartialView("_Menu", model);
        }
    }
}