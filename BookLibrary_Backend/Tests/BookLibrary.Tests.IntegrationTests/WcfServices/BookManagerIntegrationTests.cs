using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.Services.Managers;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests.WcfServices
{
    public class BookManagerIntegrationTests
    {
        [Fact]
        public async Task GetBooks_ShouldReturnBooks()
        {
            var bookManager = new BookManager();

            var libraryBooks = await bookManager.GetBooksAsync(0,0, null);
            var libraryBook = libraryBooks.FirstOrDefault();

            Assert.NotEmpty(libraryBooks);
            Assert.NotNull(libraryBook);
            Assert.NotNull(libraryBook.Isbn);
            Assert.NotNull(libraryBook.Title);
            Assert.NotNull(libraryBook.Category);
            Assert.NotNull(libraryBook.CoverLink);
        }

    }
}
