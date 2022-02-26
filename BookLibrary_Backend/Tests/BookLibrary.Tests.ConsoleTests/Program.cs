using System;
using BookLibrary.DataAccess.SQLite.Repositories;

namespace BookLibrary.Tests.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BookRepositoryIntegrationTests();
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

            Console.ReadLine();
        }
    }
}
