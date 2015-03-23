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
        private readonly ClassRepository _classRepository = new ClassRepository();

        public ActionResult List()
        {
            var subjects = _subjectRepository.GetAll();
            var model = Mapper.Map<List<SubjectModel>>(subjects);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new SubjectModel();
            PrepareSubjectModel(model);
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

            PrepareSubjectModel(model);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var subjects = _subjectRepository.GetById(id);
            if (subjects == null)
            {
                return RedirectToAction("List");
            }

            var model = Mapper.Map<SubjectModel>(subjects);
            PrepareSubjectModel(model);
            return View(model);
        }

        private void PrepareSubjectModel(SubjectModel model)
        {
            model.ClassesList = SelectList(_classRepository.GetAll(),
                c => c.Id,
                c => c.Name + (!string.IsNullOrEmpty(c.Section)
                                   ? string.Format(" ({0})", c.Section)
                                   : string.Empty));
        }
    }
}
