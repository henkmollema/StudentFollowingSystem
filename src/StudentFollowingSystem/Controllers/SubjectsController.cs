using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
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
            model.StartDateString = model.StartDate.ToString("dd-MM-yyyy HH:mm");
            model.EndDateString = model.EndDate.ToString("dd-MM-yyyy HH:mm");
            PrepareSubjectModel(model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(SubjectModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = Mapper.Map<Subject>(model);
                _subjectRepository.Update(subject);
                return RedirectToAction("List");
            }

            model.StartDateString = model.StartDate.ToString("dd-MM-yyyy HH:mm");
            model.EndDateString = model.EndDate.ToString("dd-MM-yyyy HH:mm");
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
