﻿using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();
        private readonly StudentRepository _studentRepository = new StudentRepository();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [AllowAnonymous, HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _studentRepository.GetByEmail(model.Email);
                if (student != null)
                {
                    if (Crypto.VerifyHashedPassword(student.Password, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(student.Email, true);
                        return RedirectToAction("Dashboard", "Students");
                    }
                }

                var counseler = _counselerRepository.GetByEmail(model.Email);
                if (counseler != null)
                {
                    if (Crypto.VerifyHashedPassword(counseler.Password, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(counseler.Email, true);
                        return RedirectToAction("Dashboard", "Counseler");
                    }
                }

                ModelState.AddModelError("", "Je gebruikersnaam of wachtwoord klopt niet.");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
