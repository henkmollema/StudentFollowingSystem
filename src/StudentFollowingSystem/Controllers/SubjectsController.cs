using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;
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
            //PrepareSubjectModel(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(SubjectModel model)
        {
            if (ModelState.IsValid)
            {
                var @subjects = Mapper.Map<Subject>(model);
                _subjectRepository.Add(@subjects);
                return RedirectToAction("List");
            }

            //PrepareSubjectModel(model);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var @subjects = _subjectRepository.GetById(id);
            if (@subjects == null)
            {
                // Class does not exists, show the overview page.
                return RedirectToAction("List");
            }

            var model = Mapper.Map<Subject>(@subjects);
            //PrepareClassModel(model);
            return View(model);
        }
    }
}