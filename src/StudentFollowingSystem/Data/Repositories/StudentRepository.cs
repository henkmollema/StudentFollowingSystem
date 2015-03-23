using System.Linq;
using Dapper;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class StudentRepository : RepositoryBase<Student>
    {
        /// <summary>
        /// Gets a student by its <paramref name="id"/> with its corresponding class.
        /// </summary>
        /// <param name="id">The id of the student.</param>
        /// <returns>A student.</returns>
        public override Student GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                string sql = @"
select * from Students s
left join Classes c on c.Id = s.ClassId
where s.Id = @Id";

                return con.Query<Student, Class, Student>(
                    sql,
                    (s, c) =>
                    {
                        s.Class = c;
                        return s;
                    },
                    new { Id = id }).FirstOrDefault();
            }
        }

        public override int Add(Student entity)
        {
            return base.Add(entity);
        }

        /// <summary>
        /// Gets a student by its <paramref name="email"/>.
        /// </summary>
        /// <param name="email">The email address of the student.</param>
        /// <returns>A student.</returns>
        public Student GetByEmail(string email)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Student>(s => s.Email == email).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a student by its <paramref name="studentNr"/>.
        /// </summary>
        /// <param name="studentNr">The student number of the student.</param>
        /// <returns>A student.</returns>
        public Student GetByStudentNr(int studentNr)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Student>(s => s.StudentNr == studentNr).FirstOrDefault();
            }
        }
    }
}
