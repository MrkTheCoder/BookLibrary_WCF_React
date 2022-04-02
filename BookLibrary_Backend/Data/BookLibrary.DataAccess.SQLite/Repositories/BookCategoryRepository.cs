using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
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
    }
}
