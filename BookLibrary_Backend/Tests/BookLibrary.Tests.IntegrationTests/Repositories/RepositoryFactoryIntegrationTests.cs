using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            BootContainer.Builder = Bootstrapper.LoadContainer;

            _repositoryFactory = BootContainer.Builder.Resolve<IRepositoryFactory>();
        }

        [Fact]
        public void RepositoryFactory_IEntityRepositories_ShouldReturnRelatedEntityRepository()
        {
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();
            var bookCopyRepository = _repositoryFactory.GetEntityRepository<IBookCopyRepository>();
            var bookCategoryRepository = _repositoryFactory.GetEntityRepository<IBookCategoryRepository>();

            Assert.Equal(typeof(BookRepository), bookRepository.GetType());
            Assert.True(bookRepository is BookRepository);
            Assert.Equal(typeof(BookCopyRepository), bookCopyRepository.GetType());
            Assert.True(bookCopyRepository is BookCopyRepository);
            Assert.Equal(typeof(BookCategoryRepository), bookCategoryRepository.GetType());
            Assert.True(bookCategoryRepository is BookCategoryRepository);
        }


        [Fact]
        public async Task  RepositoryFactory_BookRepositoryGetById_ShouldReturnBookWithId()
        {
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();
            var firstBook = (await bookRepository.GetAllAsync()).Single(i => i.Id == 1);
            
            var book = await bookRepository.GetByIdAsync(1);

            Assert.Equal(firstBook.Id, book.Id);
            Assert.Equal(firstBook.Isbn, book.Isbn);
            Assert.Equal(firstBook.BookCategoryId, book.BookCategoryId);
            Assert.Equal(firstBook.BookCategory.EntityId, book.BookCategory.EntityId);
        }

        [Fact]
        public async Task RepositoryFactory_BookRepositoryGetAll_ShouldReturnBooks()
        {
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();

            var books = (await bookRepository.GetAllAsync()).ToList();

            Assert.Equal(typeof(List<Book>), books.GetType());
            Assert.True(books != null);
        }

        [Fact]
        public async void RepositoryFactory_BookRepositoryAdd_ShouldAddBook()
        {
            var newBookCopy = new BookCopy { TotalCopy = 66 };
            var newBook = new Book { Isbn = "111-222", Title = "A B C" , BookCategoryId = 1, BookCopy = newBookCopy};
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();
            var bookCategoryRepository = _repositoryFactory.GetEntityRepository<IBookCategoryRepository>();

            var book = bookRepository.Add(newBook);

            var findBook = (await bookRepository.GetAllAsync()).FirstOrDefault(f => f.Id == book.Id);
            var bookCat = await bookCategoryRepository.GetByIdAsync(1);

            Assert.True(book.Id != 0);
            Assert.NotNull(findBook);
            Assert.NotNull(bookCat);
            Assert.Equal(newBook.Isbn, findBook.Isbn);
            Assert.Equal(bookCat.EntityId, findBook.BookCategoryId);
            Assert.Equal(bookCat.Name, findBook.BookCategory.Name);
            Assert.Equal(newBookCopy.TotalCopy, findBook.BookCopy.TotalCopy);

            bookRepository.Remove(book.Id);
        }

        [Fact]
        public async Task RepositoryFactory_BookRepositoryUpdate_ShouldUpdateBook()
        {
            var newBook1 = new Book { Isbn = "113-222", Title = "A B C"  , BookCategoryId = 1};
            var bookRepository = _repositoryFactory.GetEntityRepository<IBookRepository>();
            
            var book = bookRepository.Add(newBook1);
            var id = book.Id;
            
            book.Isbn = "000-000";
            var updatedBook = bookRepository.Update(book);

            var findBook = (await bookRepository.GetAllAsync()).FirstOrDefault(f => f.Id == id);
            Assert.NotNull(findBook);
            Assert.Equal(book.Isbn, findBook.Isbn);
            Assert.Equal(book.Isbn, updatedBook.Isbn);
            Assert.True(updatedBook.Id == id);

            bookRepository.Remove(id);
        }

    }
}
