﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;

namespace BookLibrary.Business.Services.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    ReleaseServiceInstanceOnTransactionComplete = false,
                    IncludeExceptionDetailInFaults = true)]
    public class BookManager : ManagerBase, IBookService
    {
        public BookManager() : base() 
        { }
        public BookManager(IRepositoryFactory resRepositoryFactory) : base(resRepositoryFactory) 
        { }

        
        public LibraryBookData[] GetLibraryBooks()
        {
            var libraryBooks = new List<LibraryBookData>();

            var bookRepository = RepositoryFactory.GetEntityRepository<IBookRepository>();
            var books = bookRepository.GetAll();

            foreach (var book in books)
            {
                var rand = new Random();

                libraryBooks.Add(new LibraryBookData
                {
                    Id = book.Id,
                    Isbn = book.Isbn,
                    Title = book.Isbn,
                    IsAvailable = rand.Next(2) >= 1
                });
            }

            return libraryBooks.ToArray();
        }
    }
}