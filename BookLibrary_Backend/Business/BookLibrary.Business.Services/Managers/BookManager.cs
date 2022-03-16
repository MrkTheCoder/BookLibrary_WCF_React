using System;
using System.Collections.Generic;
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

        
        /// <summary>
        /// Gather simple information about books and if they are available for borrowing.
        /// </summary>
        /// <returns>Array of LibraryBookData type.</returns>
        public LibraryBookData[] GetLibraryBooks()
        {
            var libraryBooks = new List<LibraryBookData>();

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var books = bookRepository.GetAll();

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
