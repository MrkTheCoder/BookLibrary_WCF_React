using BookLibrary.Business.Entities;
using Core.Common.Interfaces.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBookCategoryRepository : IRepositoryBase<BookCategory>
    {
        Task<IEnumerable<BookCategory>> GetFilteredCategories(int page, int item);
    }
}
