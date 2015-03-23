using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using StudentFollowingSystem.ViewModels;
using Mandrill;
using StudentFollowingSystem.Services;
using System;
using System.Collections.Generic;
using AutoMapper;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Controllers
{

    public class AccountController : ControllerBase
    {
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [AllowAnonymous, HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var student = StudentRepository.GetByEmail(model.Email);
                if (student != null)
                {
                    if (Crypto.VerifyHashedPassword(student.Password, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(student.Email, true);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Dashboard", "Students");
                    }
                }

                var counseler = CounselerRepository.GetByEmail(model.Email);
                if (counseler != null)
                {
                    if (Crypto.VerifyHashedPassword(counseler.Password, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(counseler.Email, true);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
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

        [AllowAnonymous]
        public ActionResult PasswordReset()
        {
            return View(new LoginModel()); 
        }

        [HttpPost]
        public ActionResult PasswordReset(PasswordResetModel model)
        {
            if (ModelState.IsValid)
            {
                var student = StudentRepository.GetByEmail(model.Email);
                if (student != null)
                {
                        var a = student.FirstName;
                        // Generate a password for the student and hash it.
                        string password = PasswordGenerator.CreateRandomPassword();
                        student.Password = Crypto.HashPassword(password);
                        StudentRepository.Update(student);

                        // Create a mail message with the password and mail it to the student.
                        var msg = new EmailMessage
                        {
                            text = string.Format("Beste {1},{0}Er is een aavraag gedaan om je wachtwoord te resetten.{0}Inlognaam: {3} {0}Wachtwoord: {2}{0}{0}",
                                Environment.NewLine,
                                student.FirstName,
                                password,
                                student.Email),
                            subject = "Studenten Volg Systeem wachtwoord reset",
                            to = new List<EmailAddress>
                                                {
                                                    new EmailAddress(student.Email, string.Format("{0} {1}", student.FirstName, student.LastName))
                                                },
                        };
                        _mailEngine.Send(msg);
                        return RedirectToAction("", "inloggen");  
          
                }               
            }
            return View(model);
        }

    }
}
