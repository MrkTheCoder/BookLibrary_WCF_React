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
            BookRepositoryIntegrationTests();

            BootContainer.Builder = Bootstrapper.Bootstrap();

            RepositoryFactoryIntegrationTests(BootContainer.Builder.Resolve<IRepositoryFactory>());

            Console.ReadLine();
        }

        private static void BookRepositoryIntegrationTests()
        {
            var bookRepository = new BookRepository();

            var books = bookRepository.GetAll();

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

        private static void RepositoryFactoryIntegrationTests(IRepositoryFactory repositoryFactory)
        {
            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();

            var books = bookRepository.GetAll();

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
