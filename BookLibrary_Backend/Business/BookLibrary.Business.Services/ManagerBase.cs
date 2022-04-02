using System.ServiceModel.Web;
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
        protected virtual int DefaultItemsPerPage => 10;
        protected readonly int[] AcceptedItemsPerPage = new[] { 10, 20, 30, 40, 50 };

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
                                         (_container = BootContainer.Builder = Bootstrapper.Bootstrapper.LoadContainer);


        protected void SetHeaders(int itemCount, int page, int itemPerPage)
        {
            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;

            ctx.OutgoingResponse.Headers.Add("X-TotalItems", itemCount.ToString());

            if (page <= 0)
                return;

            var remainItems = itemCount - (itemPerPage * page);
            if (remainItems > 0)
                ctx.OutgoingResponse.Headers.Add("X-NextPage",
                    $"?page={page + 1}&item={itemPerPage}");
            if (page > 1)
                ctx.OutgoingResponse.Headers.Add("X-PrevPage",
                    $"?page={page - 1}&item={itemPerPage}");
        }
    }
}
