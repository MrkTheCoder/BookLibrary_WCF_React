using System.Linq;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.RepositoryTests
{
    public class BookRepositoryTests
    {
        private readonly Book[] _moqBooks;
        private readonly Book _bookIdOne;

        public BookRepositoryTests()
        {
            _moqBooks = new []
            {
                new Book{Id = 1, Isbn = "111", Title = "A"},
                new Book{Id = 2, Isbn = "222", Title = "B"},
                new Book{Id = 3, Isbn = "333", Title = "C"},
                new Book{Id = 4, Isbn = "444", Title = "D"}
            };
            _bookIdOne = _moqBooks.FirstOrDefault(f => f.Id == 1);
        }

        [Fact]
        public void GetById_ShouldReturnABook()
        {
            var bookRepositoryMoq = new Mock<IBookRepository>();
            bookRepositoryMoq.Setup(s =>
                s.GetById(It.IsAny<int>()))
                .Returns<int>((id) => _moqBooks.FirstOrDefault(f => f.Id == id));
            var aBook = _moqBooks.FirstOrDefault(f => f.Id == 1);

            var bookRepository = bookRepositoryMoq.Object;

            var book = bookRepository.GetById(1);

            Assert.NotNull(book);
            Assert.Equal(aBook.Id, book.EntityId);
            Assert.Equal(aBook.Title, book.Title);
            Assert.Equal(aBook.Isbn, book.Isbn);
            Assert.Equal(aBook.EntityId, book.EntityId);
        }

        [Fact]
        public void GetAll_ShouldReturnAllBooks()
        {
            var bookRepositoryMoq = new Mock<IBookRepository>();
            bookRepositoryMoq.Setup(s =>
                s.GetAll()).Returns(_moqBooks);
            
            var bookRepository = bookRepositoryMoq.Object;

            var books = bookRepository.GetAll();
            var getBook = books.FirstOrDefault(f => f.Id == 1);

            Assert.Equal(_moqBooks.Count(), books.Count());
            Assert.NotNull(getBook);
            Assert.Equal(_bookIdOne.Id, getBook.EntityId);
            Assert.Equal(_bookIdOne.Title, getBook.Title);
            Assert.Equal(_bookIdOne.Isbn, getBook.Isbn);
            Assert.Equal(_bookIdOne.EntityId, getBook.EntityId);
        }
    }
}
