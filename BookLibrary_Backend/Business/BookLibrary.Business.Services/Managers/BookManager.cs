using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Exceptions;
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
        /// <param name="category">a string value represent book categories.</param>
        /// // <returns>Array of LibraryBookData type in page n and x items per page.</returns>
        public async Task<LibraryBookData[]> GetBooksAsync(int page, int item, string category)
        {
            var libraryBooks = new List<LibraryBookData>();

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var books = string.IsNullOrEmpty(category?.Trim())
                ? await bookRepository.GetAllAsync()
                : await bookRepository.GetAllAsync(w => w.BookCategory.Name.ToLower() == category.ToLower());

            var bookCounts = books.Count();
            var currentPages = ValidatePage(page);
            var currentItems = ValidateItemPerPage(page, item, bookCounts);
            
            SetHeaders(bookCounts, currentPages, currentItems);

            books = books
                .Skip(currentItems * (currentPages - 1))
                .Take(currentItems);

            MapBooksToLibraryBooks(books, libraryBooks);

            return libraryBooks.ToArray();
        }

        /// <summary>
        /// Detail about specific book.
        /// </summary>
        /// <param name="isbn">a string value represent ISBN-13. It should be in this format: ###-########## [3digits-10digits]</param>
        /// // <returns>Object of LibraryBookData.</returns>
        public async  Task<LibraryBookData> GetBookAsync(string isbn)
        {
            if (!VerifyIsbn(isbn))
                throw new ArgumentException("ISBN is in a wrong format.");

            var findIsbn = isbn.Length ==14 
                ? isbn
                : isbn.Insert(3, "-");

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var book = await bookRepository.GetByExpressionAsync(b => b.Isbn == findIsbn);

            if (book == null)
                throw new NotFoundException($"Book with this ISBN {isbn} did not exits!");

            return MapBookToLibraryBook(book, new Random(1));
        }

        private bool VerifyIsbn(string isbn)
        {
            var isbnPattern = @"^\d{3}-{0,1}\d{10}$";
            return !string.IsNullOrEmpty(isbn) && 
                   Regex.IsMatch(isbn, isbnPattern);
        }

        private void MapBooksToLibraryBooks(IEnumerable<Book> books, List<LibraryBookData> libraryBooks)
        {
            var rand = new Random();
            foreach (var book in books)
            {
                libraryBooks.Add(MapBookToLibraryBook(book, rand));
            }
        }

        private static LibraryBookData MapBookToLibraryBook(Book book, Random rand)
        {
            return new LibraryBookData
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Category = book.BookCategory.Name,
                CoverLink = book.CoverLinkThumbnail,
                IsAvailable = rand.Next(2) >= 1
            };
        }

        private int ValidateItemPerPage(int page, int item, int bookCounts)
        {
            var currentItems = _acceptedItemsPerPage.Contains(item)
                ? item
                : page == 0 && item == 0
                    ? bookCounts
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
                    $"?page={page + 1}&item={itemPerPage}");
            if (page > 1)
                ctx.OutgoingResponse.Headers.Add("X-PrevPage",
                    $"?page={page - 1}&item={itemPerPage}");
        }
    }
}
