using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Required]
        public int StudentNr { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Class { get; set; }

        public string Telephone { get; set; }
    }
}
