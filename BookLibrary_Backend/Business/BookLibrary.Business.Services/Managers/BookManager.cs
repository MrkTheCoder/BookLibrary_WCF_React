using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
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
        /// <param name="page">page number from 1 to n.</param>
        /// <param name="item">items per page. It can only be of these numbers: 10, 20, 30, 40, 50. (10 is default)</param>
        /// // <returns>Array of LibraryBookData type in page n and x items per page.</returns>
        public LibraryBookData[] GetBooks(int page, int item)
        {
            if (page < 1)
                page = 1;

            var libraryBooks = new List<LibraryBookData>();
            var itemsPerPage = _acceptedItemsPerPage.Contains(item)
                ? item
                : DefaultItemsPerPage;

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var books = bookRepository.GetAll()
                .Skip(itemsPerPage * (page-1))
                .Take(itemsPerPage);
            
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
            
            return libraryBooks.ToArray();
        }
    }
}
