﻿using StudentFollowingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class CounselingModel
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CounselerId { get; set; }

        public Counseler Counseler { get; set; }

        public string Comment { get; set; }

        public bool Private { get; set; }
    }
}