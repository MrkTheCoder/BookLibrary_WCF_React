using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var books = _bookRepository.GetAllAsync();
        }

        [Fact]
        public async Task GetBook_ShouldReturnABook()
        {
            var firstBook = (await _bookRepository.GetAllAsync()).FirstOrDefault();

            var bookEntity = await _bookRepository.GetByIdAsync(1);

            Assert.NotNull(firstBook);
            Assert.NotNull(bookEntity.BookCopy);
            Assert.NotNull(bookEntity.BookCategory);
            Assert.Equal(firstBook.Id, bookEntity.EntityId);
            Assert.Equal(firstBook.Isbn, bookEntity.Isbn);
        }

        [Fact]
        public async Task AddEntity_ShouldAddedToDatabase()
        {
            ResetDatabase();

            var bookEntity = _bookRepository.Add(_newBook1);

            var findBook =(await  _bookRepository.GetAllAsync()).FirstOrDefault(f => f.Isbn == _newBook1.Isbn);
            var books = _bookRepository.GetAllAsync();

            Assert.NotNull(findBook);
            Assert.Equal(_newBook1.Isbn, findBook.Isbn);
            Assert.True(bookEntity.Id != 0);

            ResetDatabase();
        }

        [Fact]
        public async Task RemoveEntity_ShouldDeleteFromDatabase()
        {
            var removingBooks = new List<Book>();
            _bookRepository.Add(_newBook1);
            
            var books =await _bookRepository.GetAllAsync();
            
            foreach (var book in books)
                if (book.Isbn == _newBook1.Isbn)
                    removingBooks.Add(book);

            foreach (var book in removingBooks)
            {
                    _bookRepository.Remove(book);
                    var findBook = (await  _bookRepository.GetAllAsync()).FirstOrDefault(f => f.Id == book.Id);
                    Assert.Null(findBook);
            }
        }


        private async Task ResetDatabase()
        {
            var removingBooks = new List<Book>();

            var books = await _bookRepository.GetAllAsync();
            foreach (var book in books)
                if (book.Isbn == _newBook1.Isbn)
                    removingBooks.Add(book);
            foreach (var book in removingBooks) 
                _bookRepository.Remove(book);

        }
    }
}
