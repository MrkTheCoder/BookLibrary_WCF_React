using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.Business.Entities;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Exceptions;
using Core.Common.Interfaces.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BookLibrary.Tests.UnitTests.WcfServices
{
    public class BookManagerTests
    {
        private List<Book> _fakeDbBooks;
        private List<Book> _fakeDbBooksForQuery;
        private readonly Book _bookIdOne;
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;
        private readonly Mock<IBookRepository> _moqBookRepository;
        private const int DefaultItemsPerPage = 10;
        private const int TwentyItemsPerPage = 20;

        public BookManagerTests()
        {
            var catOne = new BookCategory { Id = 1, RowVersion = 0, Name = "Cat one" };
            var catTwo = new BookCategory { Id = 2, RowVersion = 0, Name = "Cat two" };
            var copyOne = new BookCopy { BookId = 1, RowVersion = 0, TotalCopy = 2 };

            _fakeDbBooksForQuery = new List<Book>
            {
                new Book{Id =1, RowVersion = 0,Title = "aaaa bbbb cccc", Isbn = "111", BookCategoryId = catOne.Id, BookCategory = catOne,BookCopy = copyOne},
                new Book{Id =2, RowVersion = 0,Title = "bbbb vvvv rrrr", Isbn = "222", BookCategoryId = catOne.Id, BookCategory = catOne,BookCopy = copyOne},
                new Book{Id =3, RowVersion = 0,Title = "uuuu wwww qqqq", Isbn = "333", BookCategoryId = catOne.Id, BookCategory = catOne,BookCopy = copyOne},
                new Book{Id =4, RowVersion = 0,Title = "bbbb aaaa mmmm", Isbn = "444", BookCategoryId = catOne.Id, BookCategory = catOne,BookCopy = copyOne},
                new Book{Id =5, RowVersion = 0,Title = "bbbb aaaa zzzz", Isbn = "555", BookCategoryId = catTwo.Id, BookCategory = catTwo,BookCopy = copyOne},
            };
            
            _fakeDbBooks = FeedBooks(21);

            _bookIdOne = _fakeDbBooks.FirstOrDefault(f => f.Id == 1);


            _moqBookRepository = new Mock<IBookRepository>();

            _moqBookRepository.Setup(s => s.GetFilteredBooksAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((int page, int item, string category, string query) =>
                {
                    return _fakeDbBooks
                        .Where(w => (string.IsNullOrEmpty(category) ||
                                    w.BookCategory.Name.ToLower() == category.ToLower()) &&
                                    (string.IsNullOrEmpty(query) ||
                                     w.Title.ToLower().Contains(query.ToLower().Trim())))
                        .Skip(item * (page - 1))
                        .Take(item)
                        .ToList();
                });

            _moqBookRepository.Setup(s => s.GetCountAsync()).ReturnsAsync(_fakeDbBooks.Count);
            _moqBookRepository.Setup(s => s.GetCountAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(f =>
                    Task.FromResult(_fakeDbBooks.AsQueryable().Count(f)));

            _moqBookRepository.Setup(s => s.GetByExpressionAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(f =>
                    Task.FromResult(_fakeDbBooks.AsQueryable().FirstOrDefault(f)));

            _moqRepositoryFactory = new Mock<IRepositoryFactory>();
            _moqRepositoryFactory.Setup(s => s.GetEntityRepository<IBookRepository>())
                .Returns(_moqBookRepository.Object);

        }

        [Fact]
        public void Initialize_ShouldIoCContainerInitializeToo()
        {
            BootContainer.Builder = Bootstrapper.LoadContainer;
            var bookManager = new BookManager();

            Assert.NotNull(BootContainer.Builder);
        }

        #region GetBooks Pagination and category Tests

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_EmptyBooks_ShouldReturnEmptyArray()
        {
            _fakeDbBooks = FeedBooks(0);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null, null);

            Assert.Empty(libraryBooks);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_WithWrongValueInArgument_ThrowArgumentException()
        {
            _fakeDbBooks = FeedBooks(0);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.GetBooksAsync(-1, 0, null, null));
            await Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.GetBooksAsync(0, -1, null, null));
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.GetBooksAsync(-1, -1, null, null));
            Assert.Equal("Page & Item arguments must be zero or a positive number", exception.Message);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_EmptyBooksWithPage1_ShouldReturnEmptyArray()
        {
            _fakeDbBooks = FeedBooks(0);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(1, 0, null, null);

            Assert.Empty(libraryBooks);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_LessThan10Books_ShouldReturnAllBooks()
        {
            _fakeDbBooks = FeedBooks(8);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null, null);
            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();

            Assert.Equal(8, libraryBooks.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            var catNum = (_bookIdOne.Id % 2) != 0 ? (_bookIdOne.Id % 2) : 2;
            Assert.Equal($"cat{catNum}", actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLinkThumbnail, actualFirstBook.CoverLink);
            Assert.Equal($"http://{_bookIdOne.Title}-tn.jpg", actualFirstBook.CoverLink);

            var expectedLastBookInPage = _fakeDbBooks.Single(s => s.Id == 8);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLinkThumbnail, actualLastBook.CoverLink);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_10Books_ShouldReturn10Books()
        {
            _fakeDbBooks = FeedBooks(10);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null, null);
            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, libraryBooks.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLinkThumbnail, actualFirstBook.CoverLink);


            var expectedLastBookInPage = _fakeDbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLinkThumbnail, actualLastBook.CoverLink);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_10BooksTwiceRequest_ShouldReturn10Books()
        {
            _fakeDbBooks = FeedBooks(10);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null, null);
            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null, null);

            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, libraryBooks.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLinkThumbnail, actualFirstBook.CoverLink);


            var expectedLastBookInPage = _fakeDbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLinkThumbnail, actualLastBook.CoverLink);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_10BooksPage1ItemsMoreThanExists_ShouldReturn10Books()
        {
            _fakeDbBooks = FeedBooks(10);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(1, 20, null, null);
            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();

            Assert.Equal(DefaultItemsPerPage, libraryBooks.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLinkThumbnail, actualFirstBook.CoverLink);



            var expectedLastBookInPage = _fakeDbBooks.Single(s => s.Id == DefaultItemsPerPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLinkThumbnail, actualLastBook.CoverLink);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_11Books_ShouldReturnFirst10Books()
        {
            _fakeDbBooks = FeedBooks(11);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null, null);
            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();

            Assert.Equal(10, libraryBooks.Length);
            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLinkThumbnail, actualFirstBook.CoverLink);


            var expectedLastBookInPage = _fakeDbBooks[9];
            Assert.NotNull(expectedLastBookInPage);
            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLinkThumbnail, actualLastBook.CoverLink);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_11BooksPage2_ShouldReturn1Book()
        {
            _fakeDbBooks = FeedBooks(11);
            var firstBookOfPageTwo = _fakeDbBooks.Single(s => s.Id == 11);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(2, 0, null, null);
            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();

            Assert.Single(libraryBooks);
            Assert.NotNull(actualFirstBook);

            Assert.Equal(firstBookOfPageTwo.Isbn, actualFirstBook.Isbn);
            Assert.Equal(firstBookOfPageTwo.Title, actualFirstBook.Title);
            Assert.Equal(firstBookOfPageTwo.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(firstBookOfPageTwo.CoverLinkThumbnail, actualFirstBook.CoverLink);

            Assert.NotNull(actualLastBook);
            Assert.Equal(actualFirstBook, actualLastBook);
        }

        [Theory]
        [MemberData(nameof(DifferentPageItemCategory))]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_21Books_DifferentPageItems(int pageNumber, int itemsPerPage, string category, string query,
            int expectedItems, int expectedPage, int expectedItemsPerPage)
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(pageNumber, itemsPerPage, category, query);

            Assert.Equal(expectedItems, libraryBooks.Length);
            Assert.Equal(expectedPage, bookManager.CurrentPage);
            Assert.Equal(expectedItemsPerPage, bookManager.CurrentItemsPerPage);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Pagination")]
        public async Task GetBooks_Page1And20Items_ShouldReturn20ItemsOfPage1BooksArray()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            var expectedLastBookInPage = _fakeDbBooks.Single(s => s.Id == TwentyItemsPerPage);

            var libraryBooks = await bookManager.GetBooksAsync(1, TwentyItemsPerPage, null, null);
            var actualFirstBook = libraryBooks.FirstOrDefault();
            var actualLastBook = libraryBooks.LastOrDefault();


            Assert.Equal(TwentyItemsPerPage, libraryBooks.Length);

            Assert.NotNull(actualFirstBook);
            Assert.Equal(_bookIdOne.Isbn, actualFirstBook.Isbn);
            Assert.Equal(_bookIdOne.Title, actualFirstBook.Title);
            Assert.Equal(_bookIdOne.BookCategory.Name, actualFirstBook.Category);
            Assert.Equal(_bookIdOne.CoverLinkThumbnail, actualFirstBook.CoverLink);

            Assert.NotNull(actualLastBook);
            Assert.Equal(expectedLastBookInPage.Isbn, actualLastBook.Isbn);
            Assert.Equal(expectedLastBookInPage.Title, actualLastBook.Title);
            Assert.Equal(expectedLastBookInPage.BookCategory.Name, actualLastBook.Category);
            Assert.Equal(expectedLastBookInPage.CoverLinkThumbnail, actualLastBook.CoverLink);

            // TODO: Assert Available property
        }
        #endregion

        #region Query Tests
        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_QueryLessAllowedCharacters_ThrowArgumentException()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => 
                await bookManager.GetBooksAsync(0, 0, null, new string('A', bookManager.MinQueryLength - 1)));
            
            Assert.Equal($"Query string must be at least {bookManager.MinQueryLength} characters or more.", exception.Message);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_QueryNotExists_ThrowNotFoundException()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await bookManager.GetBooksAsync(0, 0, null, new string('*', bookManager.MinQueryLength)));

            Assert.Equal($"No search result!", exception.Message);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_CategoryExistAndQueryNotExistInCat_ThrowNotFoundException()
        {
            _fakeDbBooks = _fakeDbBooksForQuery;
            var query = "zzzz";
            var cat = "Cat one";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await bookManager.GetBooksAsync(0, 0, null, new string('*', bookManager.MinQueryLength)));

            Assert.Equal($"No search result!", exception.Message);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_QueryExistingWord_ShouldReturnBooks()
        {
            _fakeDbBooks = _fakeDbBooksForQuery;
            var query = "bbbb";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooksAsync(0, 0, null, query);
            var expected = _fakeDbBooksForQuery.Count(c => c.Title.Contains(query));

            Assert.Equal(expected, books.Length);
        }
        
        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_QueryExistingInCaseSensitiveWord_ShouldReturnBooks()
        {
            _fakeDbBooks = _fakeDbBooksForQuery;
            var query = "bBbB";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooksAsync(0, 0, null, query);
            var expected = _fakeDbBooksForQuery.Count(c => c.Title.ToLower().Contains(query.ToLower()));

            Assert.Equal(expected, books.Length);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_QueryExistingWithSpace_ShouldReturnBooks()
        {
            _fakeDbBooks = _fakeDbBooksForQuery;
            var query = "  bBbB  ";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooksAsync(0, 0, null, query);
            var expected = _fakeDbBooksForQuery.Count(c => c.Title.ToLower().Contains(query.ToLower().Trim()));

            Assert.Equal(expected, books.Length);
        }
        [Fact]
        [Trait("BookManagerTests", "GetBooks_Query")]
        public async Task GetBooks_CategoryAndQueryExistingWord_ShouldReturnBooks()
        {
            _fakeDbBooks = _fakeDbBooksForQuery;
            var query = "bbbb";
            var cat = "Cat one";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var books = await bookManager.GetBooksAsync(0, 0, cat, query);
            var expected = _fakeDbBooksForQuery.Count(c => c.Title.Contains(query) && c.BookCategory.Name==cat);

            Assert.Equal(expected, books.Length);
        }
        #endregion



        #region ISBN Tests
        [Fact]
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_IsbnExists_ReturnABook()
        {
            var isbn = "001-0000000001";
            var book = _fakeDbBooks.Single(s => s.Isbn == isbn);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBook = await bookManager.GetBookAsync(isbn);

            Assert.NotNull(libraryBook);
            Assert.Equal(book.Isbn, libraryBook.Isbn);
            Assert.Equal(book.Title, libraryBook.Title);
            Assert.Equal(book.CoverLinkOriginal, libraryBook.CoverLink);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_IsbnNotExist_throwNotFoundException()
        {
            var isbn = "000-0000000000";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                async () => await bookManager.GetBookAsync(isbn));
            Assert.Equal($"Book with this ISBN {isbn} did not exits!", exception.Message);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_SupportBothIsbnFormat_ReturnSameBook()
        {
            var isbn1 = "001-0000000001";
            var isbn2 = "0010000000001";
            var book = _fakeDbBooks.Single(s => s.Isbn == isbn1);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBook1 = await bookManager.GetBookAsync(isbn1);
            var libraryBook2 = await bookManager.GetBookAsync(isbn2);

            Assert.NotNull(libraryBook1);
            Assert.NotNull(libraryBook2);
            Assert.Equal(libraryBook1.Isbn, libraryBook2.Isbn);
            Assert.Equal(book.Isbn, libraryBook1.Isbn);
            Assert.Equal(book.Title, libraryBook1.Title);
            Assert.Equal(book.CoverLinkOriginal, libraryBook1.CoverLink);
        }
        #endregion

        #region Etag Tests
        // OperationContext tests
        // A Good way to moq WebOperationContext: https://weblogs.asp.net/cibrax/unit-tests-for-wcf
        [Fact]
        [Trait("BookManagerTests", "GetBooks_Etag")]
        public async Task GetBooks_ShouldHaveCtx()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null, null);

            Assert.NotNull(bookManager.Ctx);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Etag")]
        public async Task GetBooks_ShouldHaveEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null, null);

            Assert.NotNull(bookManager.Ctx.OutgoingResponse.ETag);
            Assert.True(!string.IsNullOrEmpty(bookManager.Ctx.OutgoingResponse.ETag));
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Etag")]
        public async Task GetBooks_TwiceSameList_ShouldHaveSameEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null, null);
            var etag1 = bookManager.Ctx.OutgoingResponse.ETag;

            await bookManager.GetBooksAsync(0, 0, null, null);
            var etag2 = bookManager.Ctx.OutgoingResponse.ETag;

            Assert.Equal(etag1, etag2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Etag")]
        public async Task GetBooks_TwiceSameListButSecondTimeDataChanged_ShouldDifferentEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null, null);
            var etag1 = bookManager.Ctx.OutgoingResponse.ETag;

            _fakeDbBooks.First().Isbn = "010101";
            
            await bookManager.GetBooksAsync(0, 0, null, null);
            var etag2 = bookManager.Ctx.OutgoingResponse.ETag;

            Assert.NotEqual(etag1, etag2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks_Etag")]
        public async Task GetBooks_TwoPages_ShouldHaveDifferentEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(1, 0, null, null);
            var etag1 = bookManager.Ctx.OutgoingResponse.ETag;

            await bookManager.GetBooksAsync(2, 0, null, null);
            var etag2 = bookManager.Ctx.OutgoingResponse.ETag;

            Assert.NotEqual(etag1, etag2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook_Etag")]
        public async Task GetBook_ShouldHaveCtx()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBookAsync("001-0000000001");

            Assert.NotNull(bookManager.Ctx);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook_Etag")]
        public async Task GetBook_ShouldHaveEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBookAsync("001-0000000001");

            Assert.NotNull(bookManager.Ctx.OutgoingResponse.ETag);
            Assert.True(!string.IsNullOrEmpty(bookManager.Ctx.OutgoingResponse.ETag));
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook_Etag")]
        public async Task GetBook_TwiceSameBook_ShouldHaveSameEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBookAsync("001-0000000001");
            var etag1 = bookManager.Ctx.OutgoingResponse.ETag;

            await bookManager.GetBookAsync("001-0000000001");
            var etag2 = bookManager.Ctx.OutgoingResponse.ETag;

            Assert.Equal(etag1, etag2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook_Etag")]
        public async Task GetBook_TwiceSameBookButBookDataChangeInSecondTime_ShouldHaveDifferentEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBookAsync("001-0000000001");
            var etag1 = bookManager.Ctx.OutgoingResponse.ETag;

            _fakeDbBooks.Single(s => s.Isbn == "001-0000000001").RowVersion++;

            await bookManager.GetBookAsync("001-0000000001");
            var etag2 = bookManager.Ctx.OutgoingResponse.ETag;

            Assert.NotEqual(etag1, etag2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook_Etag")]
        public async Task GetBook_TwoDifferentBooks_ShouldHaveDifferentEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBookAsync("001-0000000001");
            var etag1 = bookManager.Ctx.OutgoingResponse.ETag;

            await bookManager.GetBookAsync("002-0000000002");
            var etag2 = bookManager.Ctx.OutgoingResponse.ETag;

            Assert.NotEqual(etag1, etag2);
        }
        #endregion

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("123-123")]
        [InlineData("123-123456789")]
        [InlineData("12-1234567890")]
        [InlineData("123-12345678901")]
        [InlineData("12a-1234567890")]
        [InlineData("123-123H567890")]
        [InlineData("121234567890")]
        [Trait("BookManagerTests", "GetBook_Isbn")]
        public async Task GetBook_WrongIsbnFormat_ThrowNotFoundException(string isbn)
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await bookManager.GetBookAsync(isbn));
            Assert.Equal("ISBN is in a wrong format.", exception.Message);
        }


        public static IEnumerable<object[]> DifferentPageItemCategory()
        {
            //{page, item, category, query,    expectedReturnedItems, expectedPage#, expectedItemsPerPage}
            yield return new object[] { 0, 0, "", "", 10, 1, 10 };
            yield return new object[] { 0, 0, " ", "", 0, 1, 10 };
            yield return new object[] { 0, 0, "a", "", 0, 1, 10 };
            yield return new object[] { 0, 0, "cat2", "", 10, 1, 10 };
            yield return new object[] { 0, 0, "CaT2", "", 10, 1, 10 };
            yield return new object[] { 0, 20, "cat1", "", 11, 1, 20 };
            yield return new object[] { 2, 0, "cat1", "", 1, 2, 10 };
            yield return new object[] { 2, 20, "cat1", "", 11, 1, 20 };
            yield return new object[] { 3, 0, null, "", 1, 3, 10 };
            yield return new object[] { 1, 20, null, "", 20, 1, 20 };
            yield return new object[] { 1, 30, null, "", 21, 1, 30 };
            yield return new object[] { 3, 0, "", "", 1, 3, 10 };
            yield return new object[] { 3, 0, "  ", "", 0, 1, 10 };
            yield return new object[] { 4, 0, null, "", 1, 3, 10 };
            yield return new object[] { 2, 20, null, "", 1, 2, 20 };
            yield return new object[] { 3, 20, null, "", 1, 2, 20 };
            yield return new object[] { 0, 30, null, "", 21, 1, 30 };
        }

        private static List<Book> FeedBooks(int items)
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
                            Isbn = $"{i:d3}-{i:d10}",
                            Title = $"{(char)('A' + (i - 1))}",
                            BookCategory = categories[bookCategoryId - 1],
                            BookCategoryId = bookCategoryId,
                            CoverLinkThumbnail = $"http://{(char)('A' + (i - 1))}-tn.jpg",
                            CoverLinkOriginal = $"http://{(char)('A' + (i - 1))}-o.jpg"
                        }
                    );
            }

            return books;
        }
    }
}
