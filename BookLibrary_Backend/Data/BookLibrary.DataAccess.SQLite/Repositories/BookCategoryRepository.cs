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
    }
}
