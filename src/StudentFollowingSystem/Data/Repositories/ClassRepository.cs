using System.Collections.Generic;
using System.Linq;
using Dapper;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class ClassRepository : RepositoryBase<Class>
    {
        public override List<Class> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                string sql = @"
select * from Classes class
inner join Counselers counseler on class.CounselerId = counseler.Id";
                var res = con.Query<Class, Counseler, Class>(sql, (@class, counseler) =>
                                                                  {
                                                                      @class.Counseler = counseler;
                                                                      return @class;
                                                                  })
                             .ToList();

                return res;
            }
        }
    }
}
