using System;
using System.ComponentModel;
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
        /// <param name="category">a string value represent book categories.</param>
        /// <returns>a Json format of LibraryBookData array.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "books?page={page}&item={item}&category={category}", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat=WebMessageFormat.Json)]
        Task<LibraryBookData[]> GetBooksAsync(int page, int item, string category);

        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of LibraryBookData object.
        /// Description: Query database to get book detail with specified 'isbn'.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>a Json format of LibraryBookData array.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "books/{isbn}", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat=WebMessageFormat.Json)]
        Task<LibraryBookData> GetBookAsync(string isbn);
    }
}
