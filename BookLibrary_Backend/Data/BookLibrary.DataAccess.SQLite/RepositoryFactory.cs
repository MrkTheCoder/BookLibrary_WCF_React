using BookLibrary.Business.AppConfigs;
using Core.Common.Interfaces.Data;
using DryIoc;

namespace BookLibrary.DataAccess.SQLite
{
    public class RepositoryFactory : IRepositoryFactory
    {
        T IRepositoryFactory.GetEntityRepository<T>()
        {
            return BootContainer.Builder.Resolve<T>();
        }
    }
}
