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
    public class ClassController : ControllerBase
    {
        private readonly ClassRepository _classRepository = new ClassRepository();

        public ActionResult List()
        {
            var classes = Mapper.Map<List<ClassModel>>(_classRepository.GetAll());
            return View(classes);
        }

        public ActionResult Create()
        {
            var model = new ClassModel();
            PrepareClassModel(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ClassModel model)
        {
            if (ModelState.IsValid)
            {
                var @class = Mapper.Map<Class>(model);
                _classRepository.Add(@class);
                return RedirectToAction("List");
            }

            PrepareClassModel(model);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var @class = _classRepository.GetById(id);
            if (@class == null)
            {
                // Class does not exists, show the overview page.
                return RedirectToAction("List");
            }

            var model = Mapper.Map<ClassModel>(@class);
            PrepareClassModel(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ClassModel model)
        {
            if (ModelState.IsValid)
            {
                var @class = Mapper.Map<Class>(model);
                _classRepository.Update(@class);

                return RedirectToAction("List");
            }

            PrepareClassModel(model);
            return View();
        }

        public ActionResult Delete(int id)
        {
            var @class = _classRepository.GetById(id);
            if (@class == null)
            {
                // Class does not exists, show the overview page.
                return RedirectToAction("List");
            }

            var model = Mapper.Map<ClassModel>(@class);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(ClassModel model)
        {
            var @class = _classRepository.GetById(model.Id);
            if (@class != null)
            {
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
