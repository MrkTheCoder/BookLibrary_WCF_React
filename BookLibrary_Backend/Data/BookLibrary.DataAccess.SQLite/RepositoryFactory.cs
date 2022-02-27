using Core.Common.Interfaces.Data;

namespace BookLibrary.DataAccess.SQLite
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public T GetEntityRepository<T>() where T : IRepository
        {
            throw new System.NotImplementedException();
        }
    }
}
