using System.Collections.Generic;
using System.Linq;
using Dommel;

namespace StudentFollowingSystem.Data.Repositories
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        public List<TEntity> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<TEntity>().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Get<TEntity>(id);
            }
        }

        public int Add(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Insert(entity);
            }
        }

        public bool Update(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Update(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                con.Delete(entity);
            }
        }
    }
}
