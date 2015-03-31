using System.Web.Mvc;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [RoutePrefix("")]
    public class HomeController : ControllerBase
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            var model = new MenuModel();
            if (Request.IsAuthenticated && User != null && User.Identity.IsAuthenticated)
            {
                var student = StudentRepository.GetByEmail(User.Identity.Name);
                if (student != null)
                {
                    model.IsStudent = true;
                    model.Username = student.GetFullName();
                    model.Id = student.Id;
                }
                else
                {
                    var counseler = CounselerRepository.GetByEmail(User.Identity.Name);
                    if (counseler != null)
                    {
                        model.IsCounseler = true;
                        model.Username = counseler.GetFullName();
                        model.Id = counseler.Id;
                    }
                }
            }

            return PartialView("_Menu", model);
        }
    }
}
