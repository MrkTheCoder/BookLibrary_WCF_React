using BookLibrary.Business.Entities;
using Core.Common.Interfaces.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetFilteredBooksAsync(int page, int item, string category, string query);
    }
}
