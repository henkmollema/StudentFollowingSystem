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
    [AuthorizeCounseler, RoutePrefix("klas")]
    public class ClassController : ControllerBase
    {
        private readonly ClassRepository _classRepository = new ClassRepository();

        [Route("overzicht")]
        public ActionResult List()
        {
            // Get all the classes of the logged in counseler.
            var classes = Mapper.Map<List<ClassModel>>(_classRepository.GetAll().Where(c => c.CounselerId == Counseler.Id));
            return View(classes);
        }

        [Route("nieuw")]
        public ActionResult Create()
        {
            var model = new ClassModel();
            PrepareClassModel(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("nieuw")]
        public ActionResult Create(ClassModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the class view model to a domain model and add it.
                var @class = Mapper.Map<Class>(model);
                _classRepository.Add(@class);
                return RedirectToAction("List");
            }

            PrepareClassModel(model);
            return View(model);
        }

        [Route("wijzigen/{id}")]
        public ActionResult Edit(int id)
        {
            // Get the existing class.
            var @class = _classRepository.GetById(id);
            if (@class == null)
            {
                // Class does not exists, show the overview page.
                return RedirectToAction("List");
            }

            // Map the class to a view model.
            var model = Mapper.Map<ClassModel>(@class);
            PrepareClassModel(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("wijzigen/{id}")]
        public ActionResult Edit(ClassModel model)
        {
            // Get the existing class.
            var @class = _classRepository.GetById(model.Id);
            if (@class == null)
            {
                // Class does not exists, show the overview page.
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                // Map the new values from the view model into the existing class model.
                Mapper.Map(model, @class);
                _classRepository.Update(@class);

                return RedirectToAction("List");
            }

            PrepareClassModel(model);
            return View();
        }

        [Route("verwijderen/{id}")]
        public ActionResult Delete(int id)
        {
            var @class = _classRepository.GetById(id);
            if (@class == null)
            {
                // Class does not exists, show the overview page.
                return RedirectToAction("List");
            }

            // Map the domain model to a view model.
            var model = Mapper.Map<ClassModel>(@class);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("verwijderen/{id}")]
        public ActionResult Delete(ClassModel model)
        {
            var @class = _classRepository.GetById(model.Id);
            if (@class != null)
            {
                // Remove the class from the database.
                _classRepository.Delete(@class);
            }

            return RedirectToAction("List");
        }

        private void PrepareClassModel(ClassModel model)
        {
            model.CounselerList = SelectList(CounselerRepository.GetAll(),
                c => c.Id,
                c => c.GetFullName());
        }
    }
}
