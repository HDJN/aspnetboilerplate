using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Abp.Data.Repositories.NHibernate
{
    /// <summary>
    /// Base class for all repositories those uses NHibernate.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class NhRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets the NHibernate session object to perform database operations.
        /// </summary>
        protected ISession Session { get { return NhUnitOfWork.Current.Session; } }

        /// <summary>
        /// Used to get a IQueryable that is used to retrive entities from entire table.
        /// UnitOfWork attrbute must be used to be able to call this method since this method
        /// returns IQueryable and it requires open database connection to use it.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        public IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>List of all entities</returns>
        public IList<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        /// <summary>
        /// Used to run a query over entire entities.
        /// UnitOfWork attribute is not always necessery (as opposite to <see cref="GetAll"/>)
        /// if <see cref="queryMethod"/> finishes IQueryable with ToList, FirstOrDefault etc..
        /// </summary>
        /// <typeparam name="T">Type of return value of this method</typeparam>
        /// <param name="queryMethod">This method is used to query over entities</param>
        /// <returns>Query result</returns>
        public T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(Session.Query<TEntity>());
        }

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="key">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        public TEntity Get(TPrimaryKey key)
        {
            return Session.Get<TEntity>(key);
        }

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Insert(TEntity entity)
        {
            Session.Save(entity);
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(TEntity entity)
        {
            Session.Update(entity);
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public void Delete(TPrimaryKey id)
        {
            Session.Delete(Session.Load<TEntity>(id));
        }
    }
}