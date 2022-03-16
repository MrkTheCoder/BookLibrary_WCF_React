namespace Core.Common.Interfaces.Data
{
    /// <summary>
    /// Abstract Factory interface to get an Interface of EntityRepository then return its concreted class as an abstracted.
    /// </summary>
    public interface IRepositoryFactory
    {
        T GetEntityRepository<T>()
            where T : IRepository;
    }
}
