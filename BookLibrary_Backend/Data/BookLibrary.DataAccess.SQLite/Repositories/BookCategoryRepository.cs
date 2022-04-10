using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagingEntityDto<BookCategory>> GetFilteredCategories(int page, int item)
        {
            var pagingEntityDto = new PagingEntityDto<BookCategory>();

            using (var context = new BookLibraryDbContext())
            {
                var categories = await context
                    .BookCategories
                    .Include(i => i.Books)
                    .ToListAsync();
                
                var newItem = item == -1 ? categories.Count: item;
                pagingEntityDto.TotalItems = categories.Count;

                pagingEntityDto.Entities = categories
                    .Skip(newItem * (page - 1))
                    .Take(newItem)
                    .ToList();
            }

            return pagingEntityDto;
        }
    }
}
