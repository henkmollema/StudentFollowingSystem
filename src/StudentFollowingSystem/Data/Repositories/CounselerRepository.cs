using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class CounselerRepository : RepositoryBase<Counseler>
    {
        /// <summary>
        /// Gets a counseler by its <paramref name="email"/>.
        /// </summary>
        /// <param name="email">The email address of the counseler.</param>
        /// <returns>A counseler.</returns>
        public Counseler GetByEmail(string email)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Counseler>(c => c.Email == email).FirstOrDefault();
            }
        }
    }
}
