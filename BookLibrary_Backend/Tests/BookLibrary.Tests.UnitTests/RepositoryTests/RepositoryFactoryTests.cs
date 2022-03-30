using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite;
using BookLibrary.DataAccess.SQLite.Repositories;
using Core.Common.Interfaces.Data;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.RepositoryTests
{
    public class RepositoryFactoryTests
    {
        private readonly Book[] _books;
        private readonly Book _bookIdOne;

        public RepositoryFactoryTests()
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
        public void RepositoryFactoryWithIBookRepository_ShouldReturnBookRepository()
        {
            BootContainer.Builder = Bootstrapper.LoadContainer;
            IRepositoryFactory repositoryFactory = new RepositoryFactory();
            
            var bookRepository = repositoryFactory.GetEntityRepository<IBookRepository>();
            
            Assert.IsAssignableFrom<IBookRepository>(bookRepository);
            Assert.IsType<BookRepository>(bookRepository);
        }

        [Fact]
        public async Task GetById_BookRepositoryFromRepositoryFactory_ShouldReturnABook()
        {
            var bookRepositoryMoq = new Mock<IBookRepository>();
            bookRepositoryMoq.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(_books.FirstOrDefault(f => f.Id == id)));

            var repositoryFactoryMoq = new Mock<IRepositoryFactory>();
            repositoryFactoryMoq.Setup(s => s.GetEntityRepository<IBookRepository>())
                .Returns(bookRepositoryMoq.Object);

            var bookRepository = repositoryFactoryMoq.Object.GetEntityRepository<IBookRepository>();
            

            var book = await bookRepository.GetByIdAsync(1);

            Assert.Equal(_bookIdOne.Id, book.EntityId);
            Assert.Equal(_bookIdOne.Isbn, book.Isbn);
            Assert.Equal(_bookIdOne.Title, book.Title);
            Assert.Equal(_bookIdOne.EntityId, book.EntityId);
        }
    }
}
