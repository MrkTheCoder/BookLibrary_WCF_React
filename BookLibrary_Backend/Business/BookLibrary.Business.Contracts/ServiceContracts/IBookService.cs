using System.ServiceModel;
using System.ServiceModel.Web;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IBookService
    {
        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of LibraryBookData Array.
        /// Description: Query database to get all Library books information plus if it is available for borrowing.
        /// </summary>
        /// <returns>a Json format of LibraryBookData array.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "books", 
                ResponseFormat = WebMessageFormat.Json, 
                RequestFormat=WebMessageFormat.Json)]
        LibraryBookData[] GetLibraryBooks();
    }
}
