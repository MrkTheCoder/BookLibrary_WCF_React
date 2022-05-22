using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Contracts;
using BookLibrary.Business.Services.Behaviors;
using BookLibrary.Business.Services.Managers;
using Core.Common.Helpers;
using Core.Common.Interfaces.Data;
using Core.Common.Interfaces.Entities;
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
        // TODO: Remove 1 from '_acceptedItemsPerPage' at Release versions
        private readonly int[] _acceptedItemsPerPage = new[] { 1, 10, 20, 30, 40, 50 };
        private bool HasPagination { get; set; }
        
        protected IRepositoryFactory RepositoryFactory { get; set; }
        protected virtual int DefaultItemsPerPage => 10;

        // Use Ctx in Unit Tests
        public WebOperationContext Ctx => WebOperationContext.Current;

        public int CurrentItemsPerPage { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalItems { get; private set; }
        public string ETag { get; private set; }

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
            // Prepare WebOperationContext
            var factory = new ChannelFactory<IFakeService>(
                new WebHttpBinding(),
                new EndpointAddress("http://localhost:80"));
            
            OperationContext.Current = new OperationContext(factory.CreateChannel() as IContextChannel);
            RepositoryFactory = repositoryFactory;
        }

        protected void InitializePaging(int totalItems, int page, int item)
        {
            TotalItems = totalItems;
            
            HasPagination = !(GetType().Name == nameof(CategoryManager) && (page == 0 && item == 0));

            CurrentItemsPerPage = CalculateItemsPerPage(item);

            CurrentPage = CalculateCurrentPage(page, CalculateLastPage()) ;
        }

        protected void SetHeaders(IEnumerable<IEntityBase> entities, bool returnList = true)
        {
            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;
            
            SetHeadersETag(entities);

            if (returnList)
                ctx.OutgoingResponse.Headers.Add("X-TotalItems", TotalItems.ToString());

            if (!HasPagination) 
                return;

            ctx.OutgoingResponse.Headers.Add("X-CurrentPage", 
                $"?page={CurrentPage}&item={CurrentItemsPerPage}");

            var remainItems = TotalItems - (CurrentItemsPerPage * CurrentPage);
            
            if (remainItems > 0)
                ctx.OutgoingResponse.Headers.Add("X-NextPage",
                    $"?page={CurrentPage + 1}&item={CurrentItemsPerPage}");
            
            if (CurrentPage > 1)
                ctx.OutgoingResponse.Headers.Add("X-PrevPage",
                    $"?page={CurrentPage - 1}&item={CurrentItemsPerPage}");
        }

        // ========================= Private Methods =========================
        private int CalculateItemsPerPage(int item)
        {
            return HasPagination
                ? _acceptedItemsPerPage.Contains(item)
                    ? item
                    : DefaultItemsPerPage
                : TotalItems;
        }

        private int CalculateLastPage()
        {
            return CurrentItemsPerPage != 0 &&  TotalItems != 0
                ? (int)Math.Ceiling((double)TotalItems / CurrentItemsPerPage)
                : 1;
        }

        private int CalculateCurrentPage(int page, int maxPage)
        {
            return page == 0 
                ? DefaultPage 
                : page > maxPage 
                    ? maxPage
                    : page;
        }

        private void SetHeadersETag(IEnumerable<IEntityBase> entities)
        {
            ETag = CalculateETagChecksum(entities);

            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;

            LogETag(ctx);

            try // Ignore exception at unit tests
            {
                ctx.IncomingRequest.CheckConditionalRetrieve(ETag);
            }
            catch (InvalidOperationException ex)
            {
                // ignored
            }
            ctx.OutgoingResponse.SetETag(ETag);
        }

        private void LogETag(WebOperationContext ctx)
        {
            Console.WriteLine();
            Console.WriteLine(" ( ETag Parts )");
            Console.WriteLine($"\tCalculated ETag is : {ETag}");
            try
            {
                LogIncomingRequestConditions(ctx.IncomingRequest.IfNoneMatch, "IfNoneMatch");
                LogIncomingRequestConditions(ctx.IncomingRequest.IfMatch, "IfMatch");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LogIncomingRequestConditions(IEnumerable<string> conditions, string name)
        {
            if (conditions != null)
            {
                Console.WriteLine($"\tReceived '{conditions.Count()}' {name}(s) from client:");
                foreach (var item in conditions)
                {
                    Console.WriteLine($"\t\t{item}");
                }
            }
        }
        private string CalculateETagChecksum(IEnumerable<IEntityBase> entities)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var entity in entities) 
                sb.Append(entity.ETag);

            var checksum = ChecksumHelper.CreateMD5(sb.ToString());

            return checksum;
        }
    }
}
