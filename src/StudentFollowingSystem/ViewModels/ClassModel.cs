using System.Collections.Generic;
using System.Web.Mvc;

namespace StudentFollowingSystem.ViewModels
{
    public class ClassModel
    {
        public string Name { get; set; }

        public string Section { get; set; }

        public int CounselerId { get; set; }

        public CounselerModel Counseler { get; set; }

        public IEnumerable<SelectListItem> CounselerList { get; set; }
    }
}
