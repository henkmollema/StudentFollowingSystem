using System.Collections.Generic;
using System.Linq;
using Dommel_Source;

namespace StudentFollowingSystem.Data.Repositories
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        public virtual List<TEntity> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<TEntity>().ToList();
            }
        }

        public virtual TEntity GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Get<TEntity>(id);
            }
        }

        public virtual int Add(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Insert(entity);
            }
        }

        public virtual bool Update(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Update(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                con.Delete(entity);
            }
        }
    }
}
