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
    /// Managing all CROD actions on Book table in database.
    /// </summary>
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        protected override DbSet<Book> Entities(BookLibraryDbContext entityContext)
        {
            return entityContext.Books;
        }

        protected override async Task<IEnumerable<Book>> GetEntitiesAsync(BookLibraryDbContext entityContext)
        {
            return await entityContext
                .Books
                .Include(i => i.BookCategory)
                .Include(i => i.BookCopy)
                .ToListAsync();
        }

        protected override async Task<Book> GetEntityAsync(BookLibraryDbContext entityContext, int id)
        {
            return await entityContext
                .Books
                .Include(i => i.BookCategory)
                .Include(i => i.BookCopy)
                .SingleOrDefaultAsync(f => f.Id == id);
        }

        protected override async Task<Book> GetEntityAsync(BookLibraryDbContext entityContext, Expression<Func<Book, bool>> predicate)
        {
            return await entityContext
                .Books
                .Include(i => i.BookCategory)
                .Include(i => i.BookCopy)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Book>> GetFilteredBooksAsync(int page, int item, string category)
        {
            using (var context = new BookLibraryDbContext())
            {
                return await context
                    .Books
                    .Include(i => i.BookCategory)
                    .Include(i => i.BookCopy)
                    .Where(w => string.IsNullOrEmpty(category) ||
                                w.BookCategory.Name.ToLower() == category.ToLower())
                    .OrderBy(o => o.Title)
                    .Skip(item * (page - 1))
                    .Take(item)
                    .ToListAsync();
            }
        }
    }
}
