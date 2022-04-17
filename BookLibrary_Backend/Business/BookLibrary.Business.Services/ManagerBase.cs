using System;
using System.Linq;
using System.ServiceModel.Web;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Services.Behaviors;
using BookLibrary.Business.Services.Managers;
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
        private int DefaultPage => 1;
        private readonly int[] _acceptedItemsPerPage = new[] { 1, 10, 20, 30, 40, 50 };
        private bool IsRequestedAllItems { get; set; }
        
        protected IRepositoryFactory RepositoryFactory { get; set; }
        protected virtual int DefaultItemsPerPage => 10;
        
        public int CurrentItemsPerPage { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalItems { get; private set; }

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

        private void SetHeaders()
        {
            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;

            ctx.OutgoingResponse.Headers.Add("X-TotalItems", TotalItems.ToString());

            if (IsRequestedAllItems)
                return;

            ctx.OutgoingResponse.Headers.Add("X-CurrentPage", $"?page={CurrentPage}&item={CurrentItemsPerPage}");

            var remainItems = TotalItems - (CurrentItemsPerPage * CurrentPage);
            if (remainItems > 0)
                ctx.OutgoingResponse.Headers.Add("X-NextPage",
                    $"?page={CurrentPage + 1}&item={CurrentItemsPerPage}");
            if (CurrentPage > 1)
                ctx.OutgoingResponse.Headers.Add("X-PrevPage",
                    $"?page={CurrentPage - 1}&item={CurrentItemsPerPage}");
        }

        protected void InitializePaging(int totalItems, int page, int item)
        {
            IsRequestedAllItems = GetType().Name == nameof(CategoryManager) && (page == 0 && item == 0);

            TotalItems = totalItems;

            CurrentItemsPerPage = IsRequestedAllItems 
                ? TotalItems 
                : _acceptedItemsPerPage.Contains(item) 
                    ? item 
                    : DefaultItemsPerPage;

            var maxPage = CurrentItemsPerPage != 0 
                ? (int)Math.Ceiling((double)TotalItems / CurrentItemsPerPage)
                : 1;

            CurrentPage = page == 0 
                ? DefaultPage 
                : page > maxPage 
                    ? maxPage
                    : page ;

            SetHeaders();
        }
    }
}
