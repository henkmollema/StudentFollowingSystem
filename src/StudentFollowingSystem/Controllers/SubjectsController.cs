using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler, RoutePrefix("onderdelen")]
    public class SubjectsController : ControllerBase
    {
        private readonly SubjectRepository _subjectRepository = new SubjectRepository();
        private readonly ClassRepository _classRepository = new ClassRepository();

        [Route("overzicht")]
        public ActionResult List()
        {
            // Get all the subjects and map them to view models.
            var subjects = _subjectRepository.GetAll().Where(s => s.Class.CounselerId == Counseler.Id);
            var model = Mapper.Map<List<SubjectModel>>(subjects);
            return View(model);
        }

        [Route("nieuw")]
        public ActionResult Create()
        {
            var model = new SubjectModel();
            PrepareSubjectModel(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("nieuw")]
        public ActionResult Create(SubjectModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the view model to a domain model.
                var subject = Mapper.Map<Subject>(model);
                _subjectRepository.Add(subject);
                return RedirectToAction("List");
            }

            PrepareSubjectModel(model);
            return View(model);
        }

        [Route("wijzigen/{id}")]
        public ActionResult Edit(int id)
        {
            // Get the subject by its id.
            var subjects = _subjectRepository.GetById(id);
            if (subjects == null)
            {
                return RedirectToAction("List");
            }

            // Map the subject to a view model.
            var model = Mapper.Map<SubjectModel>(subjects);

            // Prepare the view model.
            model.StartDateString = model.StartDate.ToString("dd-MM-yyyy HH:mm");
            model.EndDateString = model.EndDate.ToString("dd-MM-yyyy HH:mm");
            PrepareSubjectModel(model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("wijzigen/{id}")]
        public ActionResult Edit(SubjectModel model)
        {
            // Check if the subject exisits.
            var subject = _subjectRepository.GetById(model.Id);
            if (subject == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Map the new values from the view model to the domain model.
                Mapper.Map(model, subject);
                _subjectRepository.Update(subject);
                return RedirectToAction("List");
            }

            // Prepare the view model.
            model.StartDateString = model.StartDate.ToString("dd-MM-yyyy HH:mm");
            model.EndDateString = model.EndDate.ToString("dd-MM-yyyy HH:mm");
            PrepareSubjectModel(model);
            return View(model);
        }

        private void PrepareSubjectModel(SubjectModel model)
        {
            // Create a select list for the classes.
            model.ClassesList = SelectList(_classRepository.GetAll().Where(c => c.CounselerId == Counseler.Id),
                c => c.Id,
                c => c.Name + (!string.IsNullOrEmpty(c.Section)
                                   ? string.Format(" ({0})", c.Section)
                                   : string.Empty));
        }
    }
}
