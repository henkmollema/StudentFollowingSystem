using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class CounselerController : Controller
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();

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
                var counseler = _counselerRepository.GetByEmail(model.Email);
                if (counseler != null)
                {
                    if (Crypto.VerifyHashedPassword(counseler.Password, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(counseler.Email, true);
                        return RedirectToAction("Dashboard");
                    }
                }

                ModelState.AddModelError("", "Je gebruikersnaam of wachtwoord klopt niet");
            }

            return View(model);
        }

        public ActionResult Dashboard()
        {
            var model = new DashboardModel();
            return View(model);
        }
    }
}
