using Core.Common.Interfaces.Entities;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, BookLibraryDbContext>
        where TEntity : class, IIdentifiableEntity, new()
    {
    }
}
