using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Services.Behaviors;
using Core.Common.Interfaces.Data;
using DryIoc;

namespace BookLibrary.Business.Services
{
    /// <summary>
    /// Base class for all Services. It will initialize RepositoryFactory and load IoC Container if it is not initialized yet.
    /// </summary>
    [OperationFaultHandling]
    public abstract class ManagerBase
    {
        private Container _container;
        
        protected IRepositoryFactory RepositoryFactory { get; set; }

        /// <summary>
        /// Default constructor for use in services.
        /// </summary>
        protected ManagerBase()
        {
            RepositoryFactory = Container.Resolve<IRepositoryFactory>();
        }

        /// <summary>
        /// Constructor to use in UnitTests.
        /// </summary>
        /// <param name="repositoryFactory"></param>
        protected ManagerBase(IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Give access to IoC container. If it is not initialized yet, It will load it.
        /// </summary>
        protected Container Container => _container ?? 
                                         (_container = BootContainer.Builder = Bootstrapper.Bootstrapper.Bootstrap());


    }
}
