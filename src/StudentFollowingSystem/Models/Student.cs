using System;

namespace StudentFollowingSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string SchoolEmail { get; set; }

        public string Password { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }

        public string Telephone { get; set; }

        public string StreetName { get; set; }

        public int StreetNumber { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime EnrollDate { get; set; }

        public string PreStudy { get; set; }

        public Status Status { get; set; }

        public string Details { get; set; }
    }
}
