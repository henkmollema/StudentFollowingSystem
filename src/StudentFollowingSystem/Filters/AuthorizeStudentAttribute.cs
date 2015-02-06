using System;
using System.Web;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;

namespace StudentFollowingSystem.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeStudentAttribute : AuthorizeAttribute
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }

            if (httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // If the email address can't be found in the student table, it's not a valid student.
            var student = _studentRepository.GetByEmail(httpContext.User.Identity.Name);
            return student != null;
        }
    }
}
