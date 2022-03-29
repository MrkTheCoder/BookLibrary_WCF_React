using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IBookService
    {
        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of LibraryBookData Array.
        /// Description: Query database to get 'x' items Library books information of page 'n' with borrowing availability status.
        /// </summary>
        /// <param name="page">an integer value represent the page number between: 1 to n.</param>
        /// <param name="item">an integer value represent items per page. Valid values: 10, 20, 30, 40, 50. (default: 10)</param>
        /// <returns>a Json format of LibraryBookData array.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "books?page={page}&item={item}", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat=WebMessageFormat.Json)]
        Task<LibraryBookData[]> GetBooks(int page, int item);
    }
}
