using System;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite;
using BookLibrary.DataAccess.SQLite.Repositories;
using Core.Common.Interfaces.Data;
using DryIoc;

namespace BookLibrary.Tests.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Database if not exists or if it is old version.
            CreateInitialDatabase.Initialize();
            BootContainer.Builder = Bootstrapper.LoadContainer;

            BookRepositoryIntegrationTests();


            RepositoryFactoryIntegrationTests(BootContainer.Builder.Resolve<IRepositoryFactory>());

            Console.ReadLine();
        }

        private static async void BookRepositoryIntegrationTests()
        {
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
        }

        private static async void RepositoryFactoryIntegrationTests(IRepositoryFactory repositoryFactory)
        {
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

        }
    }
}
