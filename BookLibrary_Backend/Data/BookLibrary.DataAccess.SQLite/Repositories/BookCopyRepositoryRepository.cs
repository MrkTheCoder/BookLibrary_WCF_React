using System.Collections.Generic;
using System.Linq;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    public class BookCopyRepository : RepositoryBase<BookCopy>, IBookCopyRepository
    {
        protected override BookCopy AddEntity(BookLibraryDbContext entityContext, BookCopy entity)
        {
            return entityContext
                .BookCopies
                .Add(entity)
                .Entity;
        }

        protected override BookCopy UpdateEntity(BookLibraryDbContext entityContext, BookCopy entity)
        {
            return entityContext
                .BookCopies
                .Update(entity)
                .Entity;
        }

        protected override IEnumerable<BookCopy> GetEntities(BookLibraryDbContext entityContext)
        {
            return entityContext
                .BookCopies;
        }

        protected override BookCopy GetEntity(BookLibraryDbContext entityContext, int id)
        {
            return entityContext
                .BookCopies
                .FirstOrDefault(i => i.EntityId == id);
        }
    }
}
