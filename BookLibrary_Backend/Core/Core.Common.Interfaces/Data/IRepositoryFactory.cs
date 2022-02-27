namespace Core.Common.Interfaces.Data
{
    public interface IRepositoryFactory
    {
        T GetEntityRepository<T>()
            where T : IRepository;
    }
}
