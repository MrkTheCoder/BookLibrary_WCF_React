using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using Core.Common.Interfaces.Data;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBorrowerRepository : IRepositoryBase<Borrower>
    {
        Task<IEnumerable<Borrower>> GetFilteredBorrowersAsync(int currentPage, int currentItemsPerPage);
    }
}
