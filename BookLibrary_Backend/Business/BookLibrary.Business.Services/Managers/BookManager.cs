using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Exceptions;
using Core.Common.Helpers;
using Core.Common.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace BookLibrary.Business.Services.Managers
{
    /// <summary>
    /// Book Service for managing books in database and retrieve information based on API requests.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class BookManager : ManagerBase, IBookService
    {
        public BookManager() : base()
        { }
        public BookManager(IRepositoryFactory resRepositoryFactory) : base(resRepositoryFactory)
        { }

        private readonly Random _rand = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Gather simple information about books and if they are available for borrowing.
        /// </summary>
        /// <param name="page">an integer value represent the page number between: 1 to n.</param>
        /// <param name="item">an integer value represent items per page. Valid values: 10, 20, 30, 40, 50. (default: 10)</param>
        /// <param name="category">a string value represent book categories.</param>
        /// // <returns>Array of LibraryBookData type in page n and x items per page.</returns>
        public async Task<LibraryBookData[]> GetBooksAsync(int page, int item, string category)
        {
            if (page < 0 || item < 0)
                throw new ArgumentException("Page & Item arguments must be zero or a positive number");

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();

            var totalItems = String.IsNullOrEmpty(category)
                ? await bookRepository.GetCountAsync()
                : await bookRepository.GetCountAsync(b => b.BookCategory.Name == category);

            InitializePaging(totalItems, page, item);

            var books = await bookRepository.GetFilteredBooksAsync(CurrentPage, CurrentItemsPerPage, category);


            return MapBooksToLibraryBooks(books);
        }

        /// <summary>
        /// Detail about specific book.
        /// </summary>
        /// <param name="isbn">a string value represent ISBN-13. It should be in this format: ###-########## [3digits-10digits]</param>
        /// // <returns>Object of LibraryBookData.</returns>
        public async Task<LibraryBookData> GetBookAsync(string isbn)
        {
            if (!BookHelper.VerifyIsbn(isbn))
                throw new ArgumentException("ISBN is in a wrong format.");

            var dashedIsbn = BookHelper.AddDashToIsbn(isbn);

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var book = await bookRepository.GetByExpressionAsync(b => b.Isbn == dashedIsbn);

            if (book == null)
                throw new NotFoundException($"Book with this ISBN {isbn} did not exits!");

            return MapBookToLibraryBook(book, isThumbnail: false);
        }



        private LibraryBookData[] MapBooksToLibraryBooks(IEnumerable<Book> books, bool isThumbnail = true)
        {
            var libraryBooks = new List<LibraryBookData>();

            foreach (var book in books)
            {
                libraryBooks.Add(MapBookToLibraryBook(book, isThumbnail));
            }

            return libraryBooks.ToArray();
        }

        private LibraryBookData MapBookToLibraryBook(Book book, bool isThumbnail = true)
        {
            return new LibraryBookData
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Category = book.BookCategory.Name,
                CoverLink = isThumbnail
                    ? book.CoverLinkThumbnail
                    : book.CoverLinkOriginal,
                IsAvailable = _rand.Next(2) >= 1
            };
        }
    }
}
