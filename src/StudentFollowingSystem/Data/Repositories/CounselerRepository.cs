using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class CounselerRepository : RepositoryBase<Counseler>
    {
        public Counseler GetByEmail(string email)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Counseler>(c => c.Email == email).FirstOrDefault();
            }
        }
    }
}
