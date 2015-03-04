using System.Linq;
using Dapper;
using Dommel;
using StudentFollowingSystem.Models;
using System.Collections.Generic;

namespace StudentFollowingSystem.Data.Repositories
{
    public class StudentRepository : RepositoryBase<Student>
    {
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
                    new { id }).FirstOrDefault();
            }
        }

        public Student GetByEmail(string email)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Student>(s => s.Email == email).FirstOrDefault();
            }
        }
    }
}
