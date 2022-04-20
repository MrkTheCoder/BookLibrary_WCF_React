using BookLibrary.Business.Entities;
using Core.Common.Interfaces.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBorrowerRepository : IRepositoryBase<Borrower>
    {
        Task<IEnumerable<Borrower>> GetFilteredBorrowersAsync(int currentPage, int currentItemsPerPage);
    }
}
