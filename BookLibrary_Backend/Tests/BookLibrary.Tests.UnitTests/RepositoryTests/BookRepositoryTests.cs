using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.RepositoryTests
{
    public class BookRepositoryTests
    {
        private readonly Book[] _books;
        private readonly Book _bookIdOne;

        public BookRepositoryTests()
        {
            _books = new []
            {
                new Book{Id = 1, Isbn = "111", Title = "A"},
                new Book{Id = 2, Isbn = "222", Title = "B"},
                new Book{Id = 3, Isbn = "333", Title = "C"},
                new Book{Id = 4, Isbn = "444", Title = "D"}
            };
            _bookIdOne = _books.FirstOrDefault(f => f.Id == 1);
        }

        [Fact]
        public async Task GetById_ShouldReturnABook()
        {
            var bookRepositoryMoq = new Mock<IBookRepository>();
            bookRepositoryMoq.Setup(s =>
                s.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>((id) =>Task.FromResult( _books.FirstOrDefault(f => f.Id == id)));
            var aBook = _books.FirstOrDefault(f => f.Id == 1);

            var bookRepository = bookRepositoryMoq.Object;

            var book = await bookRepository.GetByIdAsync(1);

            Assert.NotNull(book);
            Assert.Equal(aBook.Id, book.EntityId);
            Assert.Equal(aBook.Title, book.Title);
            Assert.Equal(aBook.Isbn, book.Isbn);
            Assert.Equal(aBook.EntityId, book.EntityId);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBooks()
        {
            var bookRepositoryMoq = new Mock<IBookRepository>();
            bookRepositoryMoq.Setup(s =>
                s.GetAllAsync()).ReturnsAsync(_books);
            
            var bookRepository = bookRepositoryMoq.Object;

            var books = await bookRepository.GetAllAsync();
            var getBook = books.FirstOrDefault(f => f.Id == 1);

            Assert.Equal(_books.Count(), books.Count());
            Assert.NotNull(getBook);
            Assert.Equal(_bookIdOne.Id, getBook.EntityId);
            Assert.Equal(_bookIdOne.Title, getBook.Title);
            Assert.Equal(_bookIdOne.Isbn, getBook.Isbn);
            Assert.Equal(_bookIdOne.EntityId, getBook.EntityId);
        }
    }
}
