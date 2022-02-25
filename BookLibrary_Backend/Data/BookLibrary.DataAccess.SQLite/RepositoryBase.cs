using System.Collections.Generic;
using System.Linq;
using Core.Common.Interfaces.Data;
using Core.Common.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite
{
    public abstract class RepositoryBase<TEntity, TDbContext> : IRepositoryBase<TEntity>
        where TEntity : class, IIdentifiableEntity, new()
        where TDbContext : DbContext, new()
    {

        protected abstract TEntity AddEntity(TDbContext entityContext, TEntity entity);

        protected abstract TEntity UpdateEntity(TDbContext entityContext, TEntity entity);

        protected abstract IEnumerable<TEntity> GetEntities(TDbContext entityContext);

        protected abstract TEntity GetEntity(TDbContext entityContext, int id);


        public TEntity GetById(int id)
        {
            using (var context = new TDbContext())
                return GetEntity(context, id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var context = new TDbContext())
                return GetEntities(context).ToArray().ToList();
        }

        public TEntity Add(TEntity entity)
        {
            using (var context = new TDbContext())
            {
                var updatedEntity = AddEntity(context, entity);
                context.SaveChanges();
                return updatedEntity;
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var context = new TDbContext())
            {
                var updatedEntity = UpdateEntity(context, entity);
                context.SaveChanges();
                return updatedEntity;
            }
        }

        public void Remove(TEntity entity)
        {
            using (var context = new TDbContext())
            {
                context.Entry<TEntity>(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (var context = new TDbContext())
            {
                var entity = GetEntity(context, id);
                Remove(entity);
            }
        }
    }
}
