using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.ViewModels
{
    public class ClassModel
    {
        public ClassModel()
        {
            CounselerList = new List<CounselerModel>();
        }

        public string Name { get; set; }

        public string Section { get; set; }

        public int CounselerId { get; set; }

        public CounselerModel Counseler { get; set; }

        public List<CounselerModel> CounselerList { get; private set; }
    }
}