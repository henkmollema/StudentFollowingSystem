namespace StudentFollowingSystem.Models
{
    /// <summary>
    /// Presence of a student on a subject.
    /// </summary>
    public class Presence
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        /// <summary>
        /// Gets a value indicating whether the specified student is present on the specified subject.
        /// </summary>
        public bool IsPresent(int subjectId, int studentId)
        {
            return SubjectId == subjectId && StudentId == studentId;
        }
    }
}
