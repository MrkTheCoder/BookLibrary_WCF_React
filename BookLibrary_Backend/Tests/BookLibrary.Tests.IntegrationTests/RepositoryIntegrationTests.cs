using System.Linq;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.SQLite.Repositories;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests
{
    public class RepositoryIntegrationTests
    {
        private readonly Book _newBook1;
        private readonly BookRepository _bookRepository;

        public RepositoryIntegrationTests()
        {
            _newBook1 = new Book { Isbn = "111-222", Title = "A B C" };
            _bookRepository = new BookRepository();
        }

        [Fact]
        public void CallEntityRepository_ShouldInitialDatabase()
        {
            var books = _bookRepository.GetAll();
        }

        [Fact]
        public void AddEntity_ShouldAddedToDatabase()
        {
            var bookEntity = _bookRepository.Add(_newBook1);

            var findBook = _bookRepository.GetAll().FirstOrDefault(f => f.Isbn == _newBook1.Isbn);

            Assert.NotNull(findBook);
            Assert.Equal(_newBook1.Isbn, findBook.Isbn);
            Assert.True(bookEntity.Id != 0);
        }

        [Fact]
        public void RemoveEntity_ShouldDeleteFromDatabase()
        {
            _bookRepository.Add(_newBook1);
            
            var books = _bookRepository.GetAll();
            foreach (var book in books)
            {
                if (book.Isbn == _newBook1.Isbn)
                {
                    _bookRepository.Remove(book);
                    var findBook = _bookRepository.GetAll().FirstOrDefault(f => f.Id == book.Id);
                    Assert.Null(findBook);
                }
            }
        }

    }
}
