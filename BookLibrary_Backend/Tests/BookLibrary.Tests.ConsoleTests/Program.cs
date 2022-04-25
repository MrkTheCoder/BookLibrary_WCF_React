using System;
using System.Data;
using System.Linq;
using System.Reflection;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite;
using BookLibrary.DataAccess.SQLite.Repositories;
using Core.Common.Exceptions;
using Core.Common.Interfaces.Data;
using DryIoc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Tests.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Database forcefully to reset any changes.
            CreateInitialDatabase.Initialize(isRebuildDatabase:true);
            BootContainer.Builder = Bootstrapper.LoadContainer;

            BookRepositoryIntegrationTests();


            var repositoryFactory = BootContainer.Builder.Resolve<IRepositoryFactory>();

            RepositoryFactoryIntegrationTests(repositoryFactory);

            RepositoryFactoryIntegrationTests_AddBook(repositoryFactory);

            RepositoryFactoryIntegrationTests_UpdateBook(repositoryFactory);
            
            RepositoryFactoryIntegrationTests_UpdateBorrower(repositoryFactory);
            
            RepositoryFactoryIntegrationTests_UpdateBookIncreaseVersion(repositoryFactory);

            RepositoryFactoryIntegrationTests_ConcurrencyCheck(repositoryFactory);

            Console.ReadLine();
        }

        private static async void BookRepositoryIntegrationTests()
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var bookRepository = new BookRepository();

            var books = await bookRepository.GetAllAsync();

            foreach (var book in books)
            {
                Console.WriteLine($"Id: \t{book.Id}\n\r" +
                                  $"ISBN: \t{book.Isbn}\n\r" +
                                  $"Title: \t{book.Title}\n\r" +
                                  $"EntityId: \t{book.EntityId}");
                if (book.Id != book.EntityId)
                    throw new Exception("In each Entity, EntityId property should be equal to its Id property.");
                Console.WriteLine();
            }

            Console.WriteLine($"Method '{name}' is passed");
        }

        private static async void RepositoryFactoryIntegrationTests(IRepositoryFactory repositoryFactory)
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();

            var books = await bookRepository.GetAllAsync();

            foreach (var book in books)
            {
                Console.WriteLine($"Id: \t{book.Id}\n\r" +
                                  $"ISBN: \t{book.Isbn}\n\r" +
                                  $"Title: \t{book.Title}\n\r" +
                                  $"EntityId: \t{book.EntityId}");
                if (book.Id != book.EntityId)
                    throw new Exception("In each Entity, EntityId property should be equal to its Id property.");
                Console.WriteLine();
            }

            Console.WriteLine($"Method '{name}' is passed");
        }

        private static async void RepositoryFactoryIntegrationTests_AddBook(IRepositoryFactory repositoryFactory)
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var categoryRepository = repositoryFactory.GetEntityRepository<IBookCategoryRepository>();
            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();

            var category = (await categoryRepository.GetAllAsync()).First();
            var newBook = new Book { Isbn = "222", Title = "New Book", BookCategoryId = category.Id };

            try
            {
                var addedBook = bookRepository.Add(newBook);

                var storedBook = await bookRepository.GetByExpressionAsync(b => b.Isbn == newBook.Isbn);


                if (storedBook == null) throw new NotFoundException("Added book not found");
                if (storedBook.Title != newBook.Title) throw new Exception("Different book found");
                if (addedBook.EntityId != storedBook.Id)
                    throw new Exception("Difference between updated data and stored data.");

                Console.WriteLine($"Method '{name}' is passed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Method '{name}' is FAILED!");
                throw e;
            }
        }

        private static async void RepositoryFactoryIntegrationTests_UpdateBook(IRepositoryFactory repositoryFactory)
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();
            var storedBook = (await bookRepository.GetAllAsync()).First();
            var storedBookIsbn = storedBook.Isbn;

            storedBook.Isbn = "000";

            var updatedBook = bookRepository.Update(storedBook);

            try
            {
                var getUpdatedBook = await bookRepository.GetByIdAsync(storedBook.Id);

                if (getUpdatedBook.Isbn != storedBook.Isbn) throw new Exception("Book didn't updated!");
                if (updatedBook.Isbn == storedBookIsbn) throw new Exception("Book didn't updated!");
                if (updatedBook.Isbn != storedBook.Isbn) throw new Exception("Returned Updated Entity is different from updated variable.");
   
                Console.WriteLine($"Method '{name}' is passed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Method '{name}' is FAILED!");
                throw e;
            }
        }

        private static async void RepositoryFactoryIntegrationTests_UpdateBorrower(IRepositoryFactory repositoryFactory)
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var borrowerRepository = repositoryFactory.GetEntityRepository<IBorrowerRepository>();
            var storedBorrower = (await borrowerRepository.GetAllAsync()).First();
            var storedBorrowerFirstName = storedBorrower.FirstName;

            storedBorrower.FirstName = "ABCDEFG";

            var updateBorrower = borrowerRepository.Update(storedBorrower);

            try
            {
                var getUpdatedBorrower = await borrowerRepository.GetByIdAsync(storedBorrower.Id);

                if (getUpdatedBorrower.FirstName != storedBorrower.FirstName) throw new Exception("Borrower didn't updated!");
                if (updateBorrower.FirstName == storedBorrowerFirstName) throw new Exception("Borrower didn't updated!");
                if (updateBorrower.FirstName != storedBorrower.FirstName) throw new Exception("Returned Updated Entity is different from updated variable.");
   
                Console.WriteLine($"Method '{name}' is passed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Method '{name}' is FAILED!");
                throw e;
            }
        }

        private static async void RepositoryFactoryIntegrationTests_UpdateBookIncreaseVersion(IRepositoryFactory repositoryFactory)
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();
            var storedBook = (await bookRepository.GetAllAsync()).First();
            var storedBookVersion = storedBook.Version;

            storedBook.Isbn = "111";

            var updatedBook = bookRepository.Update(storedBook);

            try
            {
                var getUpdatedBook = await bookRepository.GetByIdAsync(storedBook.Id);

                if (storedBookVersion == getUpdatedBook.Version) throw new Exception("Version Didn't increased");
                if (updatedBook.Version != getUpdatedBook.Version) throw new Exception("Version Didn't increased");
   
                Console.WriteLine($"Method '{name}' is passed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Method '{name}' is FAILED!");
                throw e;
            }
        }

        private static async void RepositoryFactoryIntegrationTests_ConcurrencyCheck(IRepositoryFactory repositoryFactory)
        {
            var name = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Name
                .Substring(1)
                .Split('>')[0];

            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();
            var storedBook1 = (await bookRepository.GetAllAsync()).First();
            var storedBook2 = (await bookRepository.GetAllAsync()).First();

            storedBook1.Isbn = "3333";

            bookRepository.Update(storedBook1);

            try
            {
                storedBook2.Isbn = "98778";
                bookRepository.Update(storedBook2);

                Console.WriteLine($"Method '{name}' is FAILED!");
                throw new Exception("Above update shouldn't be happening");
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine($"Method '{name}' is passed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Method '{name}' is FAILED!");
                throw e;
            }
        }
    }
}
