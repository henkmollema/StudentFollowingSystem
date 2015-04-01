using System.Collections.Generic;
using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>
    {
        /// <summary>
        /// Gets all the subjects joined with the class.
        /// </summary>
        public override List<Subject> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<Subject, Class, Subject>(
                    (s, c) =>
                    {
                        s.Class = c;
                        return s;
                    }).ToList();
            }
        }

        /// <summary>
        /// Gets a subject joined with the class.
        /// </summary>
        /// <param name="id">The id of the subject.</param>
        public override Subject GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                con.Get<Subject, Class, Subject>(id,
                    (s, c) =>
                    {
                        s.Class = c;
                        return s;
                    });
            }
        }
    }
}
