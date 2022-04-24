using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    /// <summary>
    /// Managing all CROD actions for Category entity.
    /// </summary>
    public class BookCategoryRepository : RepositoryBase<BookCategory>, IBookCategoryRepository
    {
        protected override DbSet<BookCategory> Entities(BookLibraryDbContext entityContext)
        {
            return entityContext.BookCategories;
        }


        protected override async Task<IEnumerable<BookCategory>> GetEntitiesAsync(BookLibraryDbContext entityContext)
        {
            return await entityContext
                .BookCategories
                .Include(i => i.Books)
                .ToListAsync();

        }

        public async Task<IEnumerable<BookCategory>> GetFilteredCategories(int page, int item)
        {
            using (var context = new BookLibraryDbContext())
            {
                return await context
                    .BookCategories
                    .Include(i => i.Books)
                    .OrderBy(o => o.Name)
                    .Skip(item * (page - 1))
                    .Take(item)
                    .ToListAsync();

            }
        }
    }
}
