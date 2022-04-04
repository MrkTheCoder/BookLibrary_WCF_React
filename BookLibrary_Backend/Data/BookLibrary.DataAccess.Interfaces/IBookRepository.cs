using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using Core.Common.Interfaces.Data;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<PagingEntityDto<Book>> GetFilteredBooksAsync(int page, int item, string category);
    }
}
