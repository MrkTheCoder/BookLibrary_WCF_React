using Core.Common.Interfaces.Data;
using Core.Common.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookLibrary.DataAccess.SQLite
{
    /// <summary>
    /// Abstract RepositoryBase class with some Concreted methods.
    /// </summary>
    /// <typeparam name="TEntity">Database entity.</typeparam>
    /// <typeparam name="TDbContext">Database DbContext.</typeparam>
    public abstract class RepositoryBase<TEntity, TDbContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntityBase, new()
        where TDbContext : DbContext, new()
    {
        protected abstract DbSet<TEntity> Entities(TDbContext entityContext);

        protected virtual TEntity AddEntity(TDbContext entityContext, TEntity entity)
        {
            return Entities(entityContext)
                .Add(entity)
                .Entity;
        }

        protected virtual TEntity UpdateEntity(TDbContext entityContext, TEntity entity)
        {
            return Entities(entityContext)
                .Update(entity)
                .Entity;
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntitiesAsync(TDbContext entityContext)
        {
            return await Entities(entityContext)
                            .ToListAsync();
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntitiesAsync(TDbContext entityContext,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities(entityContext)
                .Where(predicate)
                .ToListAsync();
        }

        protected virtual async Task<TEntity> GetEntityAsync(TDbContext entityContext, int id)
        {
            return await Entities(entityContext)
                .FindAsync(id);
        }

        protected virtual async Task<TEntity> GetEntityAsync(TDbContext entityContext,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities(entityContext)
                .FirstOrDefaultAsync(predicate);
        }

        protected virtual async Task<TEntity> GetSingleEntityAsync(TDbContext entityContext,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities(entityContext)
                .SingleOrDefaultAsync(predicate);
        }

        protected virtual async Task<bool> AnyExistsAsync(TDbContext entityContext)
        {
            return await Entities(entityContext)
                .AnyAsync();
        }

        protected virtual async Task<bool> AnyExistsAsync(TDbContext entityContext,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities(entityContext)
                .AnyAsync(predicate);
        }

        protected virtual async Task<int> GetCountAsync(TDbContext entityContext)
        {
            return await Entities(entityContext).CountAsync();

        }

        protected virtual async Task<int> GetCountAsync(TDbContext entityContext,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities(entityContext).CountAsync(predicate);
        }


        public async Task<TEntity> GetByIdAsync(int id)
        {
            using (var context = new TDbContext())
                return await GetEntityAsync(context, id);
        }

        public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TDbContext())
                return await GetEntityAsync(context, predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TDbContext())
                return await GetSingleEntityAsync(context, predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using (var context = new TDbContext())
                return (await GetEntitiesAsync(context)).ToArray().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TDbContext())
                return (await GetEntitiesAsync(context, predicate)).ToArray().ToList();
        }

        public async Task<bool> AnyExistsAsync()
        {
            using (var context = new TDbContext())
                return await AnyExistsAsync(context);
        }

        public async Task<bool> AnyExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TDbContext())
                return await AnyExistsAsync(context, predicate);
        }

        public async Task<int> GetCountAsync()
        {
            using (var context = new TDbContext())
                return await GetCountAsync(context);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TDbContext())
                return await GetCountAsync(context, predicate);
        }

        public TEntity Add(TEntity entity)
        {
            using (var context = new TDbContext())
            {
                var addEntity = AddEntity(context, entity);
                context.SaveChanges();
                return addEntity;
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

        public async void Remove(int id)
        {
            using (var context = new TDbContext())
            {
                var entity = await GetEntityAsync(context, id);
                Remove(entity);
            }
        }
    }
}
