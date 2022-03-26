using System.Collections.Generic;
using System.Linq;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Repositories
{
    public class BookCategoryRepository : RepositoryBase<BookCategory>, IBookCategoryRepository
    {
        protected override BookCategory AddEntity(BookLibraryDbContext entityContext, BookCategory entity)
        {
            return entityContext
                .BookCategories
                .Add(entity)
                .Entity;
        }

        protected override BookCategory UpdateEntity(BookLibraryDbContext entityContext, BookCategory entity)
        {
            return entityContext
                .BookCategories
                .Update(entity)
                .Entity;
        }

        protected override IEnumerable<BookCategory> GetEntities(BookLibraryDbContext entityContext)
        {
            return entityContext
                .BookCategories
                .Include(i => i.Books);
        }

        protected override BookCategory GetEntity(BookLibraryDbContext entityContext, int id)
        {
            return entityContext
                .BookCategories
                .Include(i => i.Books)
                .FirstOrDefault(f => f.Id == id);
        }
    }
}
