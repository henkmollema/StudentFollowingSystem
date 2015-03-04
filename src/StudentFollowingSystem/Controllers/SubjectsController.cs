using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFollowingSystem.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SubjectRepository _subjectRepository = new SubjectRepository();

        public ActionResult List()
        {
            var subjects = _subjectRepository.GetAll();
            var model = Mapper.Map<List<SubjectModel>>(subjects);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new SubjectModel();            
            return View(model);
        }
    }
}