using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    public class BorrowerRepository : RepositoryBase<Borrower>, IBorrowerRepository
    {
        protected override DbSet<Borrower> Entities(BookLibraryDbContext entityContext)
        {
            return entityContext.Borrowers;
        }

        public async Task<PagingEntityDto<Borrower>> GetFilteredBorrowersAsync(int page, int item)
        {
            var pagingEntityDto = new PagingEntityDto<Borrower>();

            using (var context = new BookLibraryDbContext())
            {
                var categories = await context
                    .Borrowers
                    .Include(i => i.Gender)
                    .ToListAsync();
                
                var newItem = item == -1 ? categories.Count: item;
                pagingEntityDto.TotalItems = categories.Count;

                pagingEntityDto.Entities = categories
                    .Skip(newItem * (page - 1))
                    .Take(newItem)
                    .ToList();
            }

            return pagingEntityDto;
        }
    }
}
