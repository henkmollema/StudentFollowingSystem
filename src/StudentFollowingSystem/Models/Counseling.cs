namespace StudentFollowingSystem.Models
{
    public class Counseling
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public string Counseler { get; set; }

        public string Report { get; set; }
    }
}
