using System.Collections.Generic;
using System.Linq;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite.Repositories;
using Core.Common.Interfaces.Data;
using DryIoc;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests.Repositories
{
    public class RepositoryFactoryIntegrationTests
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public RepositoryFactoryIntegrationTests()
        {
            BootContainer.Builder = Bootstrapper.Bootstrap();

            _repositoryFactory = BootContainer.Builder.Resolve<IRepositoryFactory>();
        }

        [Fact]
        public void RepositoryFactory_GetIBookRepository_ShouldReturnBookRepository()
        {
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();

            Assert.Equal(typeof(BookRepository), bookRepository.GetType());
            Assert.True(bookRepository is BookRepository);
        }

        [Fact]
        public void BookRepositoryGetAllFromFactory_ShouldReturnBooks()
        {
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();

            var books = bookRepository.GetAll().ToList();

            Assert.Equal(typeof(List<Book>), books.GetType());
            Assert.True(books != null);
        }

        [Fact]
        public void BookRepositoryAddBookFromFactory_ShouldAddBook()
        {
            var _newBook1 = new Book { Isbn = "111-222", Title = "A B C" };
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();

            var book = bookRepository.Add(_newBook1);

            var findBook = bookRepository.GetAll().FirstOrDefault(f => f.Id == book.Id);

            Assert.NotNull(findBook);
            Assert.Equal(_newBook1.Isbn, findBook.Isbn);
            Assert.True(book.Id != 0);

            bookRepository.Remove(book);
        }

        [Fact]
        public void BookRepositoryUpdateBookFromFactory_ShouldUpdateBook()
        {
            var _newBook1 = new Book { Isbn = "113-222", Title = "A B C" };
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();
            
            var book = bookRepository.Add(_newBook1);
            var id = book.Id;
            
            book.Isbn = "000-000";
            var updatedBook = bookRepository.Update(book);

            var findBook = bookRepository.GetAll().FirstOrDefault(f => f.Id == id);
            Assert.NotNull(findBook);
            Assert.Equal(book.Isbn, findBook.Isbn);
            Assert.Equal(book.Isbn, updatedBook.Isbn);
            Assert.True(updatedBook.Id == id);

            bookRepository.Remove(id);
        }
    }
}
