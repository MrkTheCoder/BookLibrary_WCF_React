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
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "categories?page={page}&item={item}")]
        [Description("It return list of categories. Two parameters: page, item.")]
        Task<BookCategoryData[]> GetCategoriesAsync(int page, int item);
    }
}
