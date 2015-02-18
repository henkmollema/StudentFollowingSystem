using System.Web.Helpers;
using System.Web.Mvc;

namespace StudentFollowingSystem.Controllers
{
    public class ToolsController : Controller
    {
        public ActionResult HashPassword(string q)
        {
            string password = Crypto.HashPassword(q);
            return Content(password);
        }
    }
}
