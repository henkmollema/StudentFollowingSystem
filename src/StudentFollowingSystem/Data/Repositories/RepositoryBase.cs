using System.Collections.Generic;
using System.Linq;
using Dommel;

namespace StudentFollowingSystem.Data.Repositories
{
    /// <summary>
    /// Serves as the base class for all repositories, providing common functionality.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets all the entities of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A collection of entities of <typeparamref name="TEntity"/>.</returns>
        public virtual List<TEntity> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<TEntity>().ToList();
            }
        }
        
        /// <summary>
        /// Gets entity of type <typeparamref name="TEntity"/> by its id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>An entity of <typeparamref name="TEntity"/> by its <paramref name="id"/>.</returns>
        public virtual TEntity GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Get<TEntity>(id);
            }
        }

        /// <summary>
        /// Adds the specified entity to the data store.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The id of the inserted entity.</returns>
        public virtual int Add(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Insert(entity);
            }
        }

        /// <summary>
        /// Updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>true of the update succeeded; otherwise, false.</returns>
        public virtual bool Update(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Update(entity);
            }
        }

        /// <summary>
        /// Deletes the specified entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                con.Delete(entity);
            }
        }
    }
}
