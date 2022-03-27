using System.Collections.Generic;
using System.Linq;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.SQLite.Repositories;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests.Repositories
{
    public class RepositoryIntegrationTests
    {
        private readonly Book _newBook1;
        private readonly BookRepository _bookRepository;

        public RepositoryIntegrationTests()
        {
            _newBook1 = new Book { Isbn = "111-222", Title = "A B C" , BookCategoryId = 1};
            _bookRepository = new BookRepository();
        }

        [Fact]
        public void CallEntityRepository_ShouldInitialDatabase()
        {
            var books = _bookRepository.GetAll();
        }

        [Fact]
        public void GetBook_ShouldReturnABook()
        {
            var firstBook = _bookRepository.GetAll().FirstOrDefault();

            var bookEntity = _bookRepository.GetById(1);

            Assert.NotNull(firstBook);
            Assert.NotNull(bookEntity.BookCopy);
            Assert.NotNull(bookEntity.BookCategory);
            Assert.Equal(firstBook.Id, bookEntity.EntityId);
            Assert.Equal(firstBook.Isbn, bookEntity.Isbn);
        }

        [Fact]
        public void AddEntity_ShouldAddedToDatabase()
        {
            ResetDatabase();

            var bookEntity = _bookRepository.Add(_newBook1);

            var findBook = _bookRepository.GetAll().FirstOrDefault(f => f.Isbn == _newBook1.Isbn);
            var books = _bookRepository.GetAll();

            Assert.NotNull(findBook);
            Assert.Equal(_newBook1.Isbn, findBook.Isbn);
            Assert.True(bookEntity.Id != 0);

            ResetDatabase();
        }

        [Fact]
        public void RemoveEntity_ShouldDeleteFromDatabase()
        {
            var removingBooks = new List<Book>();
            _bookRepository.Add(_newBook1);
            
            var books = _bookRepository.GetAll();
            
            foreach (var book in books)
                if (book.Isbn == _newBook1.Isbn)
                    removingBooks.Add(book);

            foreach (var book in removingBooks)
            {
                    _bookRepository.Remove(book);
                    var findBook = _bookRepository.GetAll().FirstOrDefault(f => f.Id == book.Id);
                    Assert.Null(findBook);
            }
        }


        private void ResetDatabase()
        {
            var removingBooks = new List<Book>();

            var books = _bookRepository.GetAll();
            foreach (var book in books)
                if (book.Isbn == _newBook1.Isbn)
                    removingBooks.Add(book);
            foreach (var book in removingBooks) 
                _bookRepository.Remove(book);

        }
    }
}
