using System.Linq;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Entities;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.WcfServices
{
    public class BookManagerTests
    {
        private readonly Book[] _books;
        private readonly Book _bookIdOne;
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;

        public BookManagerTests()
        {
            _books = new []
            {
                new Book{Id = 1, Isbn = "111", Title = "A"},
                new Book{Id = 2, Isbn = "222", Title = "B"},
                new Book{Id = 3, Isbn = "333", Title = "C"},
                new Book{Id = 4, Isbn = "444", Title = "D"}
            };
            _bookIdOne = _books.FirstOrDefault(f => f.Id == 1);


            var moqBookRepository = new Mock<IBookRepository>();
            moqBookRepository.Setup(s => s.GetAll())
                .Returns(_books);

            _moqRepositoryFactory = new Mock<IRepositoryFactory>();
            _moqRepositoryFactory.Setup(s => s.GetEntityRepository<IBookRepository>())
                .Returns(moqBookRepository.Object);

        }

        [Fact]
        public void Initialize_ShouldIoCContainerInitializeToo()
        {
            var bookManager = new BookManager();

            Assert.NotNull(BootContainer.Builder);
        }

        [Fact]
        public void GetLibraryBook_ShouldReturnBooksArray()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetLibraryBooks();
            var book = books.FirstOrDefault(f => f.Id == 1);


            Assert.True(books.Length == 4);
            Assert.NotNull(book);
            Assert.Equal(_bookIdOne.Id, book.Id);
            Assert.Equal(_bookIdOne.Isbn, book.Isbn);
            Assert.Equal(_bookIdOne.Title, book.Title);
            // TODO: Assert Available property
        }
    }
}
