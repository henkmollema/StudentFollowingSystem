using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    public class SubjectsController : ControllerBase
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(SubjectModel model)
        {
            if (ModelState.IsValid)
            {
                var subjects = Mapper.Map<Subject>(model);
                _subjectRepository.Add(subjects);
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var subjects = _subjectRepository.GetById(id);
            if (subjects == null)
            {
                return RedirectToAction("List");
            }

            var model = Mapper.Map<Subject>(subjects);
            return View(model);
        }
    }
}
