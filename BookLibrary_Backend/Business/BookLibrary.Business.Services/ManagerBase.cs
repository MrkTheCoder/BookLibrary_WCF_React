using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using BookLibrary.Business.AppConfigs;
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
        private readonly int[] _acceptedItemsPerPage = new[] { 1, 10, 20, 30, 40, 50 };
        private bool IsRequestedAllItems { get; set; }
        
        protected IRepositoryFactory RepositoryFactory { get; set; }
        protected virtual int DefaultItemsPerPage => 10;
        
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
            RepositoryFactory = repositoryFactory;
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

            var maxPage = CurrentItemsPerPage != 0 &&  TotalItems != 0
                ? (int)Math.Ceiling((double)TotalItems / CurrentItemsPerPage)
                : 1;

            CurrentPage = page == 0 
                ? DefaultPage 
                : page > maxPage 
                    ? maxPage
                    : page ;

            SetHeaders();
        }

        protected void SetETag(IEnumerable<IEntityBase> entities)
        {
            ETag = CalculateETagChecksum(entities);
            Console.WriteLine($"Calculated ETag is : {ETag}");

            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;
            
            if (ctx.IncomingRequest.IfNoneMatch != null)
            {
                Console.WriteLine();
                Console.WriteLine($"'{ctx.IncomingRequest.IfNoneMatch.Count()}' IfNoneMatch(s) received from client:");
                foreach (var item in ctx.IncomingRequest.IfNoneMatch)
                {
                    Console.WriteLine($"\t{item}");
                }
            }

            ctx.IncomingRequest.CheckConditionalRetrieve(ETag);
            ctx.OutgoingResponse.SetETag(ETag);
        }

        private string CalculateETagChecksum(IEnumerable<IEntityBase> entities)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entity in entities)
            {
                sb.Append(entity.Version);
            }

            var checksum = ChecksumHelper.CreateMD5(sb.ToString());

            return checksum;
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
    }
}
