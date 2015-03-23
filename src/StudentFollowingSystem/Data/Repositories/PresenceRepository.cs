using System.Collections.Generic;
using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class PresenceRepository : RepositoryBase<Presence>
    {
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

        public List<Presence> GetByStudent(int studentId)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Presence>(p => p.StudentId == studentId).ToList();
            }
        }
    }
}
