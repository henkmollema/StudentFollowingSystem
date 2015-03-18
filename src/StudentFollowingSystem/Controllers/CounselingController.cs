using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFollowingSystem.Controllers
{
    public class CounselingController : Controller
    {
        public ActionResult Create(int appointmentId)
        {
            var model = new CounselingModel();

            

            return View(model);
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Create(CounselingModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var subjects = Mapper.Map<Subject>(model);
        //        _subjectRepository.Add(subjects);
        //        return RedirectToAction("List");
        //    }

        //    return View(model);
        //}
    }
}