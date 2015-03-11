using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();
        private readonly StudentRepository _studentRepository = new StudentRepository();

        protected Counseler Counseler
        {
            get
            {
                if (User != null && User.Identity.IsAuthenticated)
                {
                    return _counselerRepository.GetByEmail(User.Identity.Name);
                }

                return null;
            }
        }

        protected Student Student
        {
            get
            {
                if (User != null && User.Identity.IsAuthenticated)
                {
                    return _studentRepository.GetByEmail(User.Identity.Name);
                }

                return null;
            }
        }
    }
}
