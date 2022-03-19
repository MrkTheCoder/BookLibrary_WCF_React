using BookLibrary.Business.AppConfigs;
using Core.Common.Interfaces.Data;
using DryIoc;

namespace BookLibrary.DataAccess.SQLite
{
    /// <summary>
    /// Abstract Factory class to get an Interface of EntityRepository then return its concreted class as an abstracted.
    /// </summary>

    public class RepositoryFactory : IRepositoryFactory
    {
        T IRepositoryFactory.GetEntityRepository<T>()
        {
            return BootContainer.Builder.Resolve<T>();
        }
    }
}
