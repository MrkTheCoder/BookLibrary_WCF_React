using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using Core.Common.Interfaces.Data;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBookCategoryRepository : IRepositoryBase<BookCategory>
    {
        Task<PagingEntityDto<BookCategory>> GetFilteredCategories(int page, int item);
    }
}
