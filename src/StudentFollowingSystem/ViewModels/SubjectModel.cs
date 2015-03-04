using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentFollowingSystem.ViewModels
{
    public class SubjectModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}