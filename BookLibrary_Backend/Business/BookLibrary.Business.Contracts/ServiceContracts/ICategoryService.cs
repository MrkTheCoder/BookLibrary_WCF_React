using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface ICategoryService
    {
        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of BookCategoryData Array.
        /// Description: Query database to get 'x' items of Book Categories and show them at page 'n'.
        /// </summary>
        /// <param name="page">an integer value represent the page number between: 1 to n.</param>
        /// <param name="item">an integer value represent items per page. Valid values: 10, 20, 30, 40, 50. (default: 10)</param>
        /// <returns>A Json format of BookCategoryData array.</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "categories?page={page}&item={item}")]
        Task<BookCategoryData[]> GetCategoriesAsync(int page, int item);
    }
}
