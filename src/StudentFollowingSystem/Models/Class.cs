namespace StudentFollowingSystem.Models
{
    /// <summary>
    /// A class with a counseler.
    /// </summary>
    public class Class
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Section { get; set; }

        public int CounselerId { get; set; }

        public Counseler Counseler { get; set; }
    }
}
