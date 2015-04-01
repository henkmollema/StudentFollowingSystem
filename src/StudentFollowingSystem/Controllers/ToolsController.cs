using System.Web.Helpers;
using System.Web.Mvc;

namespace StudentFollowingSystem.Controllers
{
    public class ToolsController : Controller
    {
        public ActionResult HashPassword(string q)
        {
            // Hash the querystring value.
            string password = Crypto.HashPassword(q);
            return Content(password);
        }
    }
}
