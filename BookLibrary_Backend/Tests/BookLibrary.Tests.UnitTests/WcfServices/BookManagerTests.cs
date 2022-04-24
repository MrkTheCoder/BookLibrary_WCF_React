﻿using BookLibrary.Business.AppConfigs;
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
using Castle.Components.DictionaryAdapter;
using Xunit;

namespace BookLibrary.Tests.UnitTests.WcfServices
{
    public class BookManagerTests
    {
        private List<Book> _fakeDbBooks;
        private readonly Book _bookIdOne;
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;
        private readonly Mock<IBookRepository> _moqBookRepository;
        private const int DefaultItemsPerPage = 10;
        private const int TwentyItemsPerPage = 20;

        public BookManagerTests()
        {
            _fakeDbBooks = FeedBooks(21);

            _bookIdOne = _fakeDbBooks.FirstOrDefault(f => f.Id == 1);


            _moqBookRepository = new Mock<IBookRepository>();

            _moqBookRepository.Setup(s => s.GetFilteredBooksAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((int page, int item, string category) =>
                {
                    return _fakeDbBooks
                        .Where(w => string.IsNullOrEmpty(category) ||
                                    w.BookCategory.Name.ToLower() == category.ToLower())
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

        #region GetBooks Tests

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_EmptyBooks_ShouldReturnEmptyArray()
        {
            _fakeDbBooks = FeedBooks(0);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null);

            Assert.Empty(libraryBooks);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_WithWrongValueInArgument_ThrowArgumentException()
        {
            _fakeDbBooks = FeedBooks(0);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.GetBooksAsync(-1, 0, null));
            await Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.GetBooksAsync(0, -1, null));
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.GetBooksAsync(-1, -1, null));
            Assert.Equal("Page & Item arguments must be zero or a positive number", exception.Message);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_EmptyBooksWithPage1_ShouldReturnEmptyArray()
        {
            _fakeDbBooks = FeedBooks(0);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(1, 0, null);

            Assert.Empty(libraryBooks);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_LessThan10Books_ShouldReturnAllBooks()
        {
            _fakeDbBooks = FeedBooks(8);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null);
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
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_10Books_ShouldReturn10Books()
        {
            _fakeDbBooks = FeedBooks(10);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null);
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
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_RequestBooks_ShouldHaveAnEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            Assert.True(string.IsNullOrEmpty(bookManager.ETag));

            await bookManager.GetBooksAsync(0, 0, null);

            Assert.True(!string.IsNullOrEmpty(bookManager.ETag));
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_RequestSameBooksTwice_ShouldHaveEqualEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null);
            var eTagList1 = bookManager.ETag;

            await bookManager.GetBooksAsync(0, 0, null);
            var eTagList2 = bookManager.ETag;

            Assert.Equal(eTagList1, eTagList2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_RequestTwiceForDifferentBooks_ShouldHaveDifferentEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null);
            var eTagList1 = bookManager.ETag;

            await bookManager.GetBooksAsync(2, 0, null);
            var eTagList2 = bookManager.ETag;

            Assert.NotEqual(eTagList1, eTagList2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_RequestSameListTwiceDataChangedBetweenRequests_ShouldHaveDifferentEtag()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null);
            var eTagList1 = bookManager.ETag;

            _fakeDbBooks[0].RowVersion++;
            await bookManager.GetBooksAsync(0, 0, null);
            var eTagList2 = bookManager.ETag;

            Assert.NotEqual(eTagList1, eTagList2);
        }


        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_10BooksTwiceRequest_ShouldReturn10Books()
        {
            _fakeDbBooks = FeedBooks(10);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            await bookManager.GetBooksAsync(0, 0, null);
            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null);

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
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_10BooksPage1ItemsMoreThanExists_ShouldReturn10Books()
        {
            _fakeDbBooks = FeedBooks(10);

            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(1, 20, null);
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
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_11Books_ShouldReturnFirst10Books()
        {
            _fakeDbBooks = FeedBooks(11);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(0, 0, null);
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
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_11BooksPage2_ShouldReturn1Book()
        {
            _fakeDbBooks = FeedBooks(11);
            var firstBookOfPageTwo = _fakeDbBooks.Single(s => s.Id == 11);
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(2, 0, null);
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
        [MemberData(nameof(DifferentPages))]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_21Books_DifferentPageItems(int pageNumber, int itemsPerPage, string category,
            int expectedItems, int expectedPage, int expectedItemsPerPage)
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var libraryBooks = await bookManager.GetBooksAsync(pageNumber, itemsPerPage, category);

            Assert.Equal(expectedItems, libraryBooks.Length);
            Assert.Equal(expectedPage, bookManager.CurrentPage);
            Assert.Equal(expectedItemsPerPage, bookManager.CurrentItemsPerPage);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBooks")]
        public async Task GetBooks_Page1And20Items_ShouldReturn20ItemsOfPage1BooksArray()
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            var expectedLastBookInPage = _fakeDbBooks.Single(s => s.Id == TwentyItemsPerPage);

            var libraryBooks = await bookManager.GetBooksAsync(1, TwentyItemsPerPage, null);
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


        // ===========================================================================================
        // GetBook
        // ===========================================================================================


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

        [Fact]
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_RequestingABook_ShouldProduceETag()
        {
            var isbn1 = "001-0000000001";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            
            Assert.True(string.IsNullOrEmpty(bookManager.ETag));

            await bookManager.GetBookAsync(isbn1);

            Assert.True(!string.IsNullOrEmpty(bookManager.ETag));
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_RequestingTwiceABook_ShouldProduceSameETag()
        {
            var isbn1 = "001-0000000001";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            
            await bookManager.GetBookAsync(isbn1);
            var etag1 = bookManager.ETag;
            await bookManager.GetBookAsync(isbn1);
            var etag2 = bookManager.ETag;


            Assert.Equal(etag1,etag2);
        }

        [Fact]
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_BookChangedBetweenRequestingTwiceABook_ShouldProduceSameETag()
        {
            var isbn1 = "001-0000000001";
            var bookManager = new BookManager(_moqRepositoryFactory.Object);
            
            await bookManager.GetBookAsync(isbn1);
            var etag1 = bookManager.ETag;
            await bookManager.GetBookAsync(isbn1);
            var etag2 = bookManager.ETag;


            Assert.Equal(etag1,etag2);
        }



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
        [Trait("BookManagerTests", "GetBook")]
        public async Task GetBook_WrongIsbnFormat_ThrowNotFoundException(string isbn)
        {
            var bookManager = new BookManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await bookManager.GetBookAsync(isbn));
            Assert.Equal("ISBN is in a wrong format.", exception.Message);
        }


        public static IEnumerable<object[]> DifferentPages()
        {
            //{page, item, category,    expectedReturnedItems, expectedPage#, expectedItemsPerPage}
            yield return new object[] { 0, 0, "", 10, 1, 10 };
            yield return new object[] { 0, 0, " ", 0, 1, 10 };
            yield return new object[] { 0, 0, "a", 0, 1, 10 };
            yield return new object[] { 0, 0, "cat2", 10, 1, 10 };
            yield return new object[] { 0, 0, "CaT2", 10, 1, 10 };
            yield return new object[] { 0, 20, "cat1", 11, 1, 20 };
            yield return new object[] { 2, 0, "cat1", 1, 2, 10 };
            yield return new object[] { 2, 20, "cat1", 11, 1, 20 };
            yield return new object[] { 3, 0, null, 1, 3, 10 };
            yield return new object[] { 1, 20, null, 20, 1, 20 };
            yield return new object[] { 1, 30, null, 21, 1, 30 };
            yield return new object[] { 3, 0, "", 1, 3, 10 };
            yield return new object[] { 3, 0, "  ", 0, 1, 10 };
            yield return new object[] { 4, 0, null, 1, 3, 10 };
            yield return new object[] { 2, 20, null, 1, 2, 20 };
            yield return new object[] { 3, 20, null, 1, 2, 20 };
            yield return new object[] { 0, 30, null, 21, 1, 30 };
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
