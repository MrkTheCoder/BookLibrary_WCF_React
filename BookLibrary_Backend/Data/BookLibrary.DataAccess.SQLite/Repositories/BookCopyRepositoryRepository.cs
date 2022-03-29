using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    public class BookCopyRepository : RepositoryBase<BookCopy>, IBookCopyRepository
    {
        protected override DbSet<BookCopy> Entities(BookLibraryDbContext entityContext)
        {
            return entityContext.BookCopies;
        }
    }
}
