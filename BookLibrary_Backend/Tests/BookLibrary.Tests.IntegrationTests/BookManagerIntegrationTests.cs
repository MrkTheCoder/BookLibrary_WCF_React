using System.Linq;
using BookLibrary.Business.Services.Managers;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests
{
    public class BookManagerIntegrationTests
    {
        [Fact]
        public void GetLibraryBooks_ShouldReturnBooks()
        {
            var bookManager = new BookManager();

            var libraryBooks = bookManager.GetLibraryBooks();
            var libraryBook = libraryBooks.FirstOrDefault();

            Assert.NotEmpty(libraryBooks);
            Assert.NotNull(libraryBook);
            Assert.NotNull(libraryBook.Isbn);
            Assert.NotNull(libraryBook.Title);
            Assert.True(libraryBook.Id > 0);
        }

    }
}
