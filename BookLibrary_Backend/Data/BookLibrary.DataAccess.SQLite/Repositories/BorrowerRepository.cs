using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    /// <summary>
    /// Managing all CROD actions for Borrower entity.
    /// </summary>
    public class BorrowerRepository : RepositoryBase<Borrower>, IBorrowerRepository
    {
        protected override DbSet<Borrower> Entities(BookLibraryDbContext entityContext)
        {
            return entityContext.Borrowers;
        }

        protected override async Task<Borrower> GetEntityAsync(BookLibraryDbContext entityContext, Expression<Func<Borrower, bool>> predicate)
        {
            return await Entities(entityContext)
                .Include(i => i.Gender)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Borrower>> GetFilteredBorrowersAsync(int page, int item)
        {
            using (var context = new BookLibraryDbContext())
            {
                return await context
                    .Borrowers
                    .Include(i => i.Gender)
                    .OrderBy(o => o.LastName)
                    .Skip(item * (page - 1))
                    .Take(item)
                    .ToListAsync();
            }
        }
    }
}
