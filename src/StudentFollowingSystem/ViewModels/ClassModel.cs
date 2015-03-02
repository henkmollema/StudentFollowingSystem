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
            CounselerList = new List<Counseler>();
        }

        public string Name { get; set; }

        public string Section { get; set; }

        public int CounselerId { get; set; }

        public Counseler Counseler { get; set; }

        public List<Counseler> CounselerList { get; private set; }
    }
}