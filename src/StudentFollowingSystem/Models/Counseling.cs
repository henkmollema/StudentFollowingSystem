namespace StudentFollowingSystem.Models
{
    public class Counseling
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CounselerId { get; set; }

        public Counseler Counseler { get; set; }

        public string Report { get; set; }
    }
}
