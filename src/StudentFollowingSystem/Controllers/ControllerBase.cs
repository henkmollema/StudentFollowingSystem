using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();
        private readonly StudentRepository _studentRepository = new StudentRepository();

        /// <summary>
        /// Gets an instance of the <see cref="CounselerRepository"/>.
        /// </summary>
        protected CounselerRepository CounselerRepository
        {
            get
            {
                return _counselerRepository;
            }
        }

        /// <summary>
        /// Gets an instance of the <see cref="StudentRepository"/>.
        /// </summary>
        protected StudentRepository StudentRepository
        {
            get
            {
                return _studentRepository;
            }
        }

        /// <summary>
        /// Gets the currently logged in counseler, if available. 
        /// This can be null.
        /// </summary>
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

        /// <summary>
        /// Gets the currently logged in student, if available. 
        /// This can be null.
        /// </summary>
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
        
        /// <summary>
        /// Redirects the user to the corresponding dashboard.
        /// </summary>
        public ActionResult RedirectToDashboard()
        {
            return RedirectToAction("Dashboard", Student != null ? "Students" : "Counseler");
        }

        /// <summary>
        /// Generates a full url to the specified action including the current url.
        /// </summary>
        /// <param name="action">The name of the action method.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <returns>A full url to the specified action including the current url.</returns>
        protected string FullUrl(string action, object routeValues)
        {
            return FullUrl(action, null, routeValues);
        }

        /// <summary>
        /// Generates a full url to the specified action and controller including the current url.
        /// </summary>
        /// <param name="action">The name of the action method.</param>
        /// <param name="controller">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <returns>A full url to the specified action including the current url.</returns>
        protected string FullUrl(string action, string controller, object routeValues)
        {
            string path = Url.Action(action, controller, routeValues);
            var currentUrl = Request.Url;
            string url = string.Format("{0}://{1}", currentUrl.Scheme, currentUrl.Authority);

            return url + path;
        }

        /// <summary>
        /// Creates an MVC select list item collection using the specified source and value and text selectors.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="source">The source collection for the select list items.</param>
        /// <param name="valueSelector">The value selector for the select list items.</param>
        /// <param name="textSelector">The text selector for the select list items.</param>
        /// <returns>A collection of select list items.</returns>
        protected static IEnumerable<SelectListItem> SelectList<TItem>(IEnumerable<TItem> source,
                                                                       Func<TItem, object> valueSelector,
                                                                       Func<TItem, object> textSelector)
        {
            return source.Select(x => new SelectListItem
                                          {
                                              Value = valueSelector(x).ToString(),
                                              Text = textSelector(x).ToString()
                                          });
        }
    }
}
