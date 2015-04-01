using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Mandrill;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Services;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [RoutePrefix("account")]
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
                // Check if the email exists in the students table.
                var student = StudentRepository.GetByEmail(model.Email);
                if (student != null)
                {
                    // Verify the hashed password in the database with the specified password.
                    if (Crypto.VerifyHashedPassword(student.Password, model.Password))
                    {
                        // Set an authentication cookie.
                        FormsAuthentication.SetAuthCookie(student.Email, true);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            // Redirect to the returl url in the query string, if specified.
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Dashboard", "Students");
                    }
                }

                // The email address is not from a student, 
                // check if a counseler with the email address exists.
                var counseler = CounselerRepository.GetByEmail(model.Email);
                if (counseler != null)
                {
                    // Verify the hashed password in the database with the specified password.
                    if (Crypto.VerifyHashedPassword(counseler.Password, model.Password))
                    {
                        // Set an authentication cookie.
                        FormsAuthentication.SetAuthCookie(counseler.Email, true);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            // Redirect to the returl url in the query string, if specified.
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Dashboard", "Counseler");
                    }
                }

                // Email adress is not a student nor a counseler.
                ModelState.AddModelError("", "Je gebruikersnaam of wachtwoord klopt niet.");
            }

            return View(model);
        }

        [Route("uitloggen")]
        public ActionResult Logout()
        {
            // Remove the auth cookie.
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeStudent, Route("wachtwoord-wijzigen")]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [AuthorizeStudent, HttpPost, Route("wachtwoord-wijzigen")]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var student = Student;
                string password = Student.Password;

                // Verify the old password.
                if (Crypto.VerifyHashedPassword(password, model.OldPassword))
                {
                    // Hash and salt the new password and save it in the database.
                    student.Password = Crypto.HashPassword(model.NewPassword);
                    StudentRepository.Update(student);
                    model.Success = true;
                }
                else
                {
                    // Passwords don't match.
                    ModelState.AddModelError("", "Oude wachtwoord komt niet overeen");
                }
            }

            return View(model);
        }

        [AllowAnonymous, Route("wachtwoord-vergeten")]
        public ActionResult PasswordReset()
        {
            return View(new PasswordResetModel());
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, Route("wachtwoord-vergeten")]
        public ActionResult PasswordReset(PasswordResetModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the student exists.
                var student = StudentRepository.GetByEmail(model.Email);
                if (student != null)
                {
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
                                                   new EmailAddress(student.Email, string.Format("{0} {1}", student.FirstName, student.LastName)),
                                                   new EmailAddress(student.SchoolEmail, string.Format("{0} {1}", student.FirstName, student.LastName), "cc"),
                                               },
                                  };
                    _mailEngine.Send(msg);
                }
            }

            model.Success = true;
            return View(model);
        }
    }
}
