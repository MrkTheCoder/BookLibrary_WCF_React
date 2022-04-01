using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(_dbBooks);

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
        public async Task GetBook_EmptyBooks_ShouldReturnEmptyArray()
        {
            var dbBooks = FeedBooks(0);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(0, 0, null);

            Assert.Empty(books);
        }

        [Fact]
        public async Task GetBook_EmptyBooksWithPage1_ShouldReturnEmptyArray()
        {
            var dbBooks = FeedBooks(0);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(1, 0, null);

            Assert.Empty(books);
        }


        [Fact]
        public async Task GetBook_LessThan10Books_ShouldReturnAllBooks()
        {
            var dbBooks = FeedBooks(8);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(0, 0, null);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(8, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            var catNum = (_bookIdOne.Id % 2) != 0 ? (_bookIdOne.Id % 2) : 2;
            Assert.Equal($"cat{catNum}", actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLink, actualFirstBook.CoverLink);
            Assert.Equal($"http://{_bookIdOne.Title}.jpg", actualFirstBook.CoverLink);

            var expectedLastBookInPage = dbBooks.Single(s => s.Id == 8);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLink, actualLastBook.CoverLink);
        }

        [Fact]
        public async Task GetBook_10Books_ShouldReturn10Books()
        {
            var dbBooks = FeedBooks(10);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(0, 0, null);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLink, actualFirstBook.CoverLink);


            var expectedLastBookInPage = dbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLink, actualLastBook.CoverLink);
        }


        [Fact]
        public async Task GetBook_10BooksPage1ItemsMoreThanExists_ShouldReturn10Books()
        {
            var dbBooks = FeedBooks(10);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(1, 20, null);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLink, actualFirstBook.CoverLink);



            var expectedLastBookInPage = dbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLink, actualLastBook.CoverLink);
        }

        [Fact]
        public async Task GetBook_11Books_ShouldReturnFirst10Books()
        {
            var dbBooks = FeedBooks(11);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(0, 0, null);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();

            Assert.Equal(dbBooks.Length, books.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLink, actualFirstBook.CoverLink);


            var expectedLastBookInPage = dbBooks.LastOrDefault();
            Assert.NotNull(expectedLastBookInPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLink, actualLastBook.CoverLink);
        }


        [Fact]
        public async Task GetBook_11BooksPage2_ShouldReturn1Book()
        {
            var dbBooks = FeedBooks(11);
            _moqBookRepository.Setup(s => s.GetAllAsync())
                .ReturnsAsync(dbBooks);
            var firstBookOfPageTwo = dbBooks.Single(s => s.Id == 11);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(2, 0, null);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();

            Assert.Single(books);
            Assert.NotNull(actualFirstBook);

            Assert.Equal(firstBookOfPageTwo.Isbn, actualFirstBook.Isbn);
            Assert.Equal(firstBookOfPageTwo.Title, actualFirstBook.Title);
            Assert.Equal(firstBookOfPageTwo.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(firstBookOfPageTwo.CoverLink, actualFirstBook.CoverLink);

            Assert.NotNull(actualLastBook);
            Assert.Equal(actualFirstBook, actualLastBook);
        }

        [Theory]
        [MemberData(nameof(DifferentPages))]
        public async Task GetBook_21Books_DifferentPageItems(int pageNumber, int itemsPerPage, string category, int expectedItems)
        {
            _moqBookRepository.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(
                    i => Task
                        .FromResult(
                            _dbBooks.Where(w => w.BookCategory.Name.ToLower() == category.ToLower())
                            )
                    );

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooks(pageNumber, itemsPerPage, category);

            Assert.Equal(expectedItems, books.Length);
            // TODO: Assert Available property
        }

        [Fact]
        public async Task GetBook_Page1And20Items_ShouldReturn20ItemsOfPage1BooksArray()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            var expectedLastBookInPage = _dbBooks.Single(s => s.Id == TwentyItemsPerPage);

            var books = await bookManager.GetBooks(1, TwentyItemsPerPage, null);
            var actualFirstBook = books.FirstOrDefault();
            var actualLastBook = books.LastOrDefault();


            Assert.Equal(TwentyItemsPerPage, books.Length);

            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLink, actualFirstBook.CoverLink);

            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLink, actualLastBook.CoverLink);

            // TODO: Assert Available property
        }

        public static IEnumerable<object[]> DifferentPages()
        {
            yield return new object[] { 0, -1, null, 10 };
            yield return new object[] { -1, 0, null, 10 };
            yield return new object[] { -1, -1, null, 10 };
            yield return new object[] { 0, 0, "", 21 };
            yield return new object[] { -1, -1, "cat1", 10 };
            yield return new object[] { -1, -1, "cat", 0 };
            yield return new object[] { -1, -1, "a", 0 };
            yield return new object[] { 0, 0, "a", 0 };
            yield return new object[] { 0, 0, "cat2", 10 };
            yield return new object[] { 0, 0, "CaT2", 10 };
            yield return new object[] { 0, 20, "cat1", 11 };
            yield return new object[] { 2, 0, "cat1", 1 };
            yield return new object[] { 2, 20, "cat1", 0 };
            yield return new object[] { 3, 0, null, 1 };
            yield return new object[] { 3, 0, "", 1 };
            yield return new object[] { 3, 0, "  ", 1 };
            yield return new object[] { 4, 0, null, 0 };
            yield return new object[] { 2, 20, null, 1 };
            yield return new object[] { 3, 20, null, 0 };
            yield return new object[] { 0, 30, null, 21 };
        }

        private static Book[] FeedBooks(int items)
        {
            var books = new List<Book>();
            var categories = new List<BookCategory>
            {
                new BookCategory { Id = 1, Name = "cat1"},
                new BookCategory { Id = 2, Name = "cat2"},
                new BookCategory { Id = 3, Name = "cat3"}
            };

            for (int i = 1; i <= items; i++)
            {
                var bookCategoryId = (i % 2) != 0
                    ? (i % 2)
                    : 2;
                books
                    .Add(
                        new Book
                        {
                            Id = i,
                            Isbn = $"{i:d3}",
                            Title = $"{(char)('A' + (i - 1))}",
                            BookCategory = categories[bookCategoryId - 1],
                            BookCategoryId = bookCategoryId,
                            CoverLink = $"http://{(char)('A' + (i - 1))}.jpg"
                        }
                    );
            }

            return books.ToArray();
        }
    }
}
