using System.Web.Mvc;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class CounselerController : Controller
    {
        public ActionResult Dashboard()
        {
            var model = new CounselerDashboardModel();
            return View(model);
        }
    }
}
