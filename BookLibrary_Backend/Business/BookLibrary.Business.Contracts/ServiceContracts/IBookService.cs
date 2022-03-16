using System.ServiceModel;
using System.ServiceModel.Web;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IBookService
    {
        [OperationContract]
        [WebGet(UriTemplate = "LibraryBooks", 
                ResponseFormat = WebMessageFormat.Json, 
                RequestFormat=WebMessageFormat.Json)]
        LibraryBookData[] GetLibraryBooks();
    }
}
