using System.Collections.Generic;
using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class PresenceRepository : RepositoryBase<Presence>
    {
        /// <summary>
        /// Get all the presence records joined with the subject and student.
        /// </summary>
        public override List<Presence> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<Presence, Subject, Student, Presence>(
                    (presence, subject, student) =>
                    {
                        presence.Subject = subject;
                        presence.Student = student;
                        return presence;
                    }).ToList();
            }
        }

        /// <summary>
        /// Gets a presence record by its id joined with the subject and student.
        /// </summary>
        /// <param name="id">The id of the presence record.</param>
        public override Presence GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Get<Presence, Subject, Student, Presence>(id,
                    (presence, subject, student) =>
                    {
                        presence.Subject = subject;
                        presence.Student = student;
                        return presence;
                    });
            }
        }

        /// <summary>
        /// Gets all the presence records for a student.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        public List<Presence> GetByStudent(int studentId)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Presence>(p => p.StudentId == studentId).ToList();
            }
        }
    }
}
