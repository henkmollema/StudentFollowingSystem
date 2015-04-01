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
                // Is the logged in user a student?
                var student = Student;
                if (student != null)
                {
                    // Add student data to the view model.
                    model.IsStudent = true;
                    model.Username = student.GetFullName();
                    model.Id = student.Id;
                }
                else
                {
                    // Is the logged in user a counseler?
                    var counseler = Counseler;
                    if (counseler != null)
                    {
                        // Add counseler data to the view model.
                        model.IsCounseler = true;
                        model.Username = counseler.GetFullName();
                        model.Id = counseler.Id;
                    }
                }
            }

            // Render the menu partial view.
            return PartialView("_Menu", model);
        }
    }
}
