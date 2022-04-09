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
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "borrowers?page={page}&item={item}")]
        Task<BorrowerData[]> GetBorrowersAsync(int page = 0, int item = 0);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "borrowers/{email}")]
        Task<BorrowerData> GetBorrowerAsync(string email);
    }
}
