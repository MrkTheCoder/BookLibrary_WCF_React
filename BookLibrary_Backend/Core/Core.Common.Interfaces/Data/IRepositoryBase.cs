using System.Collections.Generic;
using Core.Common.Interfaces.Entities;

namespace Core.Common.Interfaces.Data
{
    public interface IRepositoryBase
    {
    }

    public interface IRepositoryBase<TEntity> : IRepositoryBase
        where TEntity : class, IIdentifiableEntity, new()
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Remove(TEntity entity);
        void Remove(int id);
    }
}
