using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.SQLite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests.WcfServices
{
    public class BookManagerIntegrationTests
    {
        public BookManagerIntegrationTests()
        {
            // Create Database if not exists or if it is old version.
            CreateInitialDatabase.Initialize(true);

            BootContainer.Builder = Bootstrapper.LoadContainer;
        }

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
