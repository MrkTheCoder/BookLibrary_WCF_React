using System.Collections.Generic;
using System.Linq;
using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    public class BookRepository : RepositoryBase<Book>
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
                .FirstOrDefault(f => f.Id == entity.Id);
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
