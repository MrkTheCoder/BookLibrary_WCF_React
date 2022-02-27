using System.Collections.Generic;
using Core.Common.Interfaces.Entities;

namespace Core.Common.Interfaces.Data
{
    public interface IRepositoryBase<TEntity> : IRepository
        where TEntity : class, IIdentifiableEntity
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Remove(TEntity entity);
        void Remove(int id);
    }
}