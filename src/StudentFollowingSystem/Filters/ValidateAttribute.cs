using System;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Validatr.Filters
{
    /// <summary>
    /// Replaces the response of the current action with a JSON 
    /// representation of the model state when it's invalid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid)
            {
                return;
            }

            var serializedModelState = JsonConvert.SerializeObject(
                filterContext.Controller.ViewData.ModelState,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var result = new ContentResult
                             {
                                 Content = serializedModelState,
                                 ContentType = "application/json"
                             };

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            filterContext.Result = result;
        }
    }
}
