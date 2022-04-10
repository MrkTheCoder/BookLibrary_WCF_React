using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IBorrowerService
    {
        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of BorrowerData Array.
        /// Description: Query database to get 'x' items of Borrowers and show them at page 'n'.
        /// </summary>
        /// <param name="page">an integer value represent the page number between: 1 to n.</param>
        /// <param name="item">an integer value represent items per page. Valid values: 10, 20, 30, 40, 50. (default: 10)</param>
        /// <returns>A Json format of BorrowerData array.</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "borrowers?page={page}&item={item}")]
        Task<BorrowerData[]> GetBorrowersAsync(int page = 0, int item = 0);

        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of a single BorrowerData object.
        /// Description: Query database to find a borrower based on their email address.
        /// </summary>
        /// <param name="email">a string value represent email address.</param>
        /// <returns>A Json format of a BorrowerData object.</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "borrowers/{email}")]
        Task<BorrowerData> GetBorrowerAsync(string email);
    }
}
