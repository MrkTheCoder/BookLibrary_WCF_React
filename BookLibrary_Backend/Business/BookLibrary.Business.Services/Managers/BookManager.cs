using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;

namespace BookLibrary.Business.Services.Managers
{
    /// <summary>
    /// Book Service for managing books in database and retrieve information based on API requests.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BookManager : ManagerBase, IBookService
    {
        public BookManager() : base()
        { }
        public BookManager(IRepositoryFactory resRepositoryFactory) : base(resRepositoryFactory)
        { }

        private const int DefaultItemsPerPage = 10;
        private readonly int[] _acceptedItemsPerPage = new[] { 10, 20, 30, 40, 50 };
        /// <summary>
        /// Gather simple information about books and if they are available for borrowing.
        /// </summary>
        /// <param name="page">an integer value represent the page number between: 1 to n.</param>
        /// <param name="item">an integer value represent items per page. Valid values: 10, 20, 30, 40, 50. (default: 10)</param>
        /// // <returns>Array of LibraryBookData type in page n and x items per page.</returns>
        public LibraryBookData[] GetBooks(int page, int item)
        {
            var libraryBooks = new List<LibraryBookData>();

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var books = bookRepository.GetAll();

            var bookCounts = books.Count();
            var currentPages = page > 0
                ? page
                : 1;
            var currentItems = _acceptedItemsPerPage.Contains(item)
                ? item
                : page == 0 && item == 0 ? bookCounts : DefaultItemsPerPage;
            books = books
                .Skip(currentItems * (currentPages - 1))
                .Take(currentItems);

            var rand = new Random();
            foreach (var book in books)
            {
                libraryBooks.Add(new LibraryBookData
                {
                    Id = book.Id,
                    Isbn = book.Isbn,
                    Title = book.Title,
                    IsAvailable = rand.Next(2) >= 1
                });
            }

            SetHeaders(bookCounts, currentPages, currentItems);

            return libraryBooks.ToArray();
        }

        public LibraryBookData[] GetBooks(bool useWrapper, int page, int item)
        {
            throw new NotImplementedException();
        }

        private void SetHeaders(int itemCount, int page, int itemPerPage)
        {
            if (!(WebOperationContext.Current is WebOperationContext ctx))
                return;

            ctx.OutgoingResponse.Headers.Add("X-TotalItems", itemCount.ToString());

            if (page <= 0)
                return;

            var remainItems = itemCount - (itemPerPage * page);
            if (remainItems > 0)
                ctx.OutgoingResponse.Headers.Add("X-NextPage",
                    $"/api/books?page={page + 1}&item={itemPerPage}");
            if (page > 1)
                ctx.OutgoingResponse.Headers.Add("X-PrevPage",
                    $"/api/books?page={page - 1}&item={itemPerPage}");
        }
    }
}
