using System.Linq;
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
        protected IRepositoryFactory RepositoryFactory { get; set; }
        protected virtual int DefaultItemsPerPage => 10;
        protected int CurrentItemsPerPage { get; private set; }
        protected int CurrentPage { get; private set; }
        private readonly int[] _acceptedItemsPerPage = new[] { 10, 20, 30, 40, 50 };

        /// <summary>
        /// Default constructor for use in services.
        /// </summary>
        protected ManagerBase()
        {
            RepositoryFactory = BootContainer.Builder.Resolve<IRepositoryFactory>();
        }

        /// <summary>
        /// Constructor to use in UnitTests.
        /// </summary>
        /// <param name="repositoryFactory"></param>
        protected ManagerBase(IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        protected void SetHeaders(int itemCount, int page, int itemPerPage)
        {
            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;

            ctx.OutgoingResponse.Headers.Add("X-TotalItems", itemCount.ToString());

            if (itemPerPage <= 0)
                return;

            var remainItems = itemCount - (itemPerPage * page);
            if (remainItems > 0)
                ctx.OutgoingResponse.Headers.Add("X-NextPage",
                    $"?page={page + 1}&item={itemPerPage}");
            if (page > 1)
                ctx.OutgoingResponse.Headers.Add("X-PrevPage",
                    $"?page={page - 1}&item={itemPerPage}");
        }

        protected void InitializePaging(int page, int item)
        {
            CurrentPage = ValidatePage(page);
            CurrentItemsPerPage = ValidateItemPerPage(page, item);
        }

        private int ValidateItemPerPage(int page, int item)
        {
            var currentItems = _acceptedItemsPerPage.Contains(item)
                ? item
                : page == 0 && item == 0
                    ? -1
                    : DefaultItemsPerPage;
            return currentItems;
        }

        private int ValidatePage(int page)
        {
            var currentPages = page > 0
                ? page
                : 1;
            return currentPages;
        }
    }
}
