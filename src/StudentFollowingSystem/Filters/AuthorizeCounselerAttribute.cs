using System;
using System.Web;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;

namespace StudentFollowingSystem.Filters
{
    /// <summary>
    /// Authorizes a counseler when executing an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeCounselerAttribute : AuthorizeAttribute
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();

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

            // If the email address can't be found in the counseler table, it's not a valid counseler.
            var counseler = _counselerRepository.GetByEmail(httpContext.User.Identity.Name);
            return counseler != null;
        }
    }
}
