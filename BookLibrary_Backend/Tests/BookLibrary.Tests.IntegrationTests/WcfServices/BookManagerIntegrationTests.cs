using System.Linq;
using BookLibrary.Business.Services.Managers;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests.WcfServices
{
    public class BookManagerIntegrationTests
    {
        [Fact]
        public void GetBooks_ShouldReturnBooks()
        {
            var bookManager = new BookManager();

            var libraryBooks = bookManager.GetBooks(0,0);
            var libraryBook = libraryBooks.FirstOrDefault();

            Assert.NotEmpty(libraryBooks);
            Assert.NotNull(libraryBook);
            Assert.NotNull(libraryBook.Isbn);
            Assert.NotNull(libraryBook.Title);
            Assert.True(libraryBook.Id > 0);
        }

    }
}
