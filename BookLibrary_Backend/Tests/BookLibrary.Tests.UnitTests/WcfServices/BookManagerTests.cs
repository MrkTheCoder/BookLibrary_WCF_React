using System.Collections.Generic;
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
        private readonly Book[] _dbBooks;
        private readonly Book _bookIdOne;
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;
        private Mock<IBookRepository> _moqBookRepository;
        private const int DefaultItemsPerPage = 10;
        private const int TwentyItemsPerPage = 20;

        public BookManagerTests()
        {
            _dbBooks = FeedBooks(21).ToArray();

            _bookIdOne = _dbBooks.FirstOrDefault(f => f.Id == 1);


            _moqBookRepository = new Mock<IBookRepository>();
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(_dbBooks);

            _moqRepositoryFactory = new Mock<IRepositoryFactory>();
            _moqRepositoryFactory.Setup(s => s.GetEntityRepository<IBookRepository>())
                .Returns(_moqBookRepository.Object);

        }

        [Fact]
        public void Initialize_ShouldIoCContainerInitializeToo()
        {
            var bookManager = new BookManager();

            Assert.NotNull(BootContainer.Builder);
        }


        [Fact]
        public void GetBook_EmptyBooks_ShouldReturnEmptyArray()
        {
            var dbBooks = FeedBooks(0);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(0,0);

            Assert.Empty(books);
        }

        [Fact]
        public void GetBook_EmptyBooksWithPage1_ShouldReturnEmptyArray()
        {
            var dbBooks = FeedBooks(0);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(1,0);

            Assert.Empty(books);
        }

        
        [Fact]
        public void GetBook_LessThan10Books_ShouldReturnAllBooks()
        {
            var dbBooks = FeedBooks(8);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(0,0);
            var actualFirstBook = books.FirstOrDefault(f => f.Id == 1);
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(8, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Id, actualFirstBook.Id);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            // TODO: Assert Available property
            
            var expectedLastBookInPage = dbBooks.Single(s => s.Id == 8);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Id, actualLastBook.Id);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
        }

        [Fact]
        public void GetBook_10Books_ShouldReturn10Books()
        {
            var dbBooks = FeedBooks(10);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(0,0);
            var actualFirstBook = books.FirstOrDefault(f => f.Id == 1);
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Id, actualFirstBook.Id);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            // TODO: Assert Available property

            
            var expectedLastBookInPage = dbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Id, actualLastBook.Id);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
        }


        [Fact]
        public void GetBook_10BooksPage1ItemsMoreThanExists_ShouldReturn10Books()
        {
            var dbBooks = FeedBooks(10);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(1,20);
            var actualFirstBook = books.FirstOrDefault(f => f.Id == 1);
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Id, actualFirstBook.Id);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            // TODO: Assert Available property

            
            var expectedLastBookInPage = dbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Id, actualLastBook.Id);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
        }

        [Fact]
        public void GetBook_11Books_ShouldReturnFirst10Books()
        {
            var dbBooks = FeedBooks(11);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(0,0);
            var actualFirstBook = books.FirstOrDefault(f => f.Id == 1);
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Id, actualFirstBook.Id);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            // TODO: Assert Available property

            
            var expectedLastBookInPage = dbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Id, actualLastBook.Id);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
        }


        [Fact]
        public void GetBook_11BooksPage2_ShouldReturn1Book()
        {
            var dbBooks = FeedBooks(11);
            _moqBookRepository.Setup(s => s.GetAll())
                .Returns(dbBooks);
            var firstBookOfPageTwo = dbBooks.Single(s => s.Id == 11);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = bookManager.GetBooks(2,0);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(1, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(firstBookOfPageTwo.Id, actualFirstBook.Id);
            Assert.Equal(firstBookOfPageTwo.Isbn, actualFirstBook.Isbn);
            Assert.Equal(firstBookOfPageTwo.Title, actualFirstBook.Title);
            // TODO: Assert Available property
            Assert.NotNull(actualLastBook);
            Assert.Equal(actualFirstBook, actualLastBook);
        }

        [Theory]
        [MemberData(nameof(DifferentPages))]
        public void GetBook_21Books_DifferentPageItems(int pageNumber, int itemsPerPage, int expectedItems)
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
        
            var books = bookManager.GetBooks(pageNumber,itemsPerPage);
        
            Assert.Equal(expectedItems, books.Length);
            // TODO: Assert Available property
        }
        
        [Fact]
        public void GetBook_Page1And20Items_ShouldReturn20ItemsOfPage1BooksArray()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            var expectedLastBookInPage = _dbBooks.Single(s => s.Id == TwentyItemsPerPage);

            var books = bookManager.GetBooks(1, TwentyItemsPerPage);
            var actualFirstBook = books.FirstOrDefault(f => f.Id == 1);
            var actualLastBook = books.LastOrDefault();


            Assert.Equal(TwentyItemsPerPage, books.Length);
            
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Id, actualFirstBook.Id);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);

            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Id, actualLastBook.Id);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);

            // TODO: Assert Available property
        }
        
        public static IEnumerable<object[]> DifferentPages()
        {
            yield return new object[] {0, -1, 10} ;
            yield return new object[] {-1, 0, 10} ;
            yield return new object[] {3, 0, 1} ;
            yield return new object[] {4, 0, 0} ;
            yield return new object[] {2, 20, 1} ;
            yield return new object[] {3, 20, 0} ;
            yield return new object[] {0, 30, 21} ;
        }

        private static Book[] FeedBooks(int items)
        {
            var books = new List<Book>();

            for (int i = 1; i <= items; i++)
                books.Add(new Book { Id = i, Isbn = $"{i:d3}", Title = $"{'A' + (i - 1)}" });

            return books.ToArray();
        }
    }
}
