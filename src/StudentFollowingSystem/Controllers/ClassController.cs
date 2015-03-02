using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult List()
        {
            var classes = Mapper.Map<List<ClassModel>>(_classRepository.GetAll());
            return View(classes);
        }
    }
}