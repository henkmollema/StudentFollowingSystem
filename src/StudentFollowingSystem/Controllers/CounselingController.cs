using AutoMapper;
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CounselingModel model)
        {
            if (ModelState.IsValid)
            {
                // Map counseling view model to domain model.
                var counseling = Mapper.Map<Counseling>(model);
                // todo: add counseling repository to class
                //_counselingRepostiory.Add(counseling);

                // todo: redirect to details
                //return RedirectToAction("List");
            }
        
            return View(model);
        }
    }
}