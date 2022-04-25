using Core.Common.Interfaces.Entities;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    /// <summary>
    /// Make RepositoryBase simpler by removing defining DbContext and make defining each EntityRepository class easier with less parameter.
    /// </summary>
    /// <typeparam name="TEntity">Database entity.</typeparam>
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, BookLibraryDbContext>
        where TEntity : class, IEntityBase, new()
    {
    }
}
