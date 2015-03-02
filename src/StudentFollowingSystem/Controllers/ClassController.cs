using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class ClassController : Controller
    {
        private readonly ClassRepository _classRepository = new ClassRepository();
        private readonly CounselerRepository _counselerRepository = new CounselerRepository();

        public ActionResult List()
        {
            var classes = Mapper.Map<List<ClassModel>>(_classRepository.GetAll());
            return View(classes);
        }

        public ActionResult Create()
        {
            var model = new ClassModel
                            {
                                CounselerList = Mapper.Map<List<CounselerModel>>(_counselerRepository.GetAll()).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.FullName })
                            };

            return View(model);
        }
    }
}
