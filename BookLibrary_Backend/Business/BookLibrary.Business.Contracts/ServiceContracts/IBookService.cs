using System.ServiceModel;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IBookService
    {
        [OperationContract]
        LibraryBookData[] GetLibraryBooks();
    }
}
