using System.Linq;
using System.ServiceModel;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Client.Entities;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests.Hosts
{
    /// <summary>
    /// Most do these before run the test:
    ///     1) Update and sync this Project app.config with the host.
    ///         1.1) This property must set to false: <webHttpBinding><binding crossDomainScriptAccessEnabled="false"/></webHttpBinding>>
    ///         1.2) Address must be look like the host base address.
    ///     2) Run the host before run the tests.
    /// </summary>
    public class WcfConsoleHostTests
    {
        [Fact]
        public void OpenProxy_ShouldAccessTheService()
        {
            var channel = new ChannelFactory<IBookService>("");
            
            var proxy = channel.CreateChannel();

            (proxy as ICommunicationObject)?.Open();

            channel.Close();
        }

        [Fact]
        public void AccessLibraryBooks_ShouldReturnBooks()
        {
            var channel = new ChannelFactory<IBookService>("");
            
            var proxy = channel.CreateChannel();

            var books = proxy.GetLibraryBooks();

            Assert.NotNull(books);

            channel.Close();
        }
    }
}
