using System.Collections.Generic;
using System.Linq;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    /// <summary>
    /// Managing all CROD actions on Book table in database.
    /// </summary>
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        protected override Book AddEntity(BookLibraryDbContext entityContext, Book entity)
        {
            return entityContext
                .Books
                .Add(entity)
                .Entity;
        }

        protected override Book UpdateEntity(BookLibraryDbContext entityContext, Book entity)
        {
            return entityContext
                .Books
                .Update(entity)
                .Entity;
        }

        protected override IEnumerable<Book> GetEntities(BookLibraryDbContext entityContext)
        {
            return entityContext
                .Books;
        }

        protected override Book GetEntity(BookLibraryDbContext entityContext, int id)
        {
            return entityContext
                .Books
                .FirstOrDefault(f => f.Id == id);
        }
    }
}
