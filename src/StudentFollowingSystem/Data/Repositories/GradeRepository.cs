using System.Collections.Generic;
using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class GradeRepository : RepositoryBase<Grade>
    {
        /// <summary>
        /// Gets all the grades joined with exam unit and student.
        /// </summary>
        /// <returns>All the grades joined with exam unit and student.</returns>
        public override List<Grade> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<Grade, ExamUnit, Student, Grade>(
                    (g, eu, s) =>
                    {
                        g.ExamUnit = eu;
                        g.Student = s;
                        return g;
                    }).ToList();
            }
        }

        /// <summary>
        /// Gets all the grades for a student.
        /// </summary>
        /// <param name="id">The id of the student.</param>
        /// <returns>All the grades of the student.</returns>
        public List<Grade> GetByStudent(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<Grade, ExamUnit, Student, Grade>(
                    (g, eu, s) =>
                    {
                        g.ExamUnit = eu;
                        g.Student = s;
                        return g;
                    }, buffered: false)
                          .Where(g => g.StudentId == id)
                          .ToList();
            }
        }
    }
}
