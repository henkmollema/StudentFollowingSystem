using System.Linq;
using Dommel_Source;
using StudentFollowingSystem.Models;

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
