using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Common.Interfaces.Entities;

namespace Core.Common.Interfaces.Data
{
    /// <summary>
    /// Define general actions for each entity CROD actions.
    /// </summary>
    /// <typeparam name="TEntity">Database entity.</typeparam>
    public interface IRepositoryBase<TEntity> : IRepository
        where TEntity : class, IIdentifiableEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyExistsAsync();
        Task<bool> AnyExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> GetCountAsync();
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Remove(TEntity entity);
        void Remove(int id);
    }
}