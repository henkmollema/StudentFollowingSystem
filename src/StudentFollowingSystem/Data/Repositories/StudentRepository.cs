using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;
using System.Collections.Generic;

namespace StudentFollowingSystem.Data.Repositories
{
    public class StudentRepository : RepositoryBase<Student>
    {
        public Student GetByEmail(string email)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Student>(s => s.Email == email).FirstOrDefault();
            }
        }
    }
}
