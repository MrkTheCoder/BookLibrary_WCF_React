﻿using System;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookLibrary.DataAccess.Dto;

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

        public async Task<PagingEntityDto<Book>> GetFilteredBooksAsync(int page, int item, string category)
        {
            var filteredBooksDto = new PagingEntityDto<Book>();

            using (var context = new BookLibraryDbContext())
            {
                var books = await context
                    .Books
                    .Include(i => i.BookCategory)
                    .Include(i => i.BookCopy)
                    .Where(w => string.IsNullOrEmpty(category) ||
                                w.BookCategory.Name.ToLower() == category.ToLower())
                    .ToListAsync();
                
                filteredBooksDto.TotalItems = books.Count;
                
                var newItem = item == -1 ? filteredBooksDto.TotalItems : item;

                filteredBooksDto.Entities = books
                    .Skip(newItem * (page - 1))
                    .Take(newItem)
                    .ToList();
            }

            return filteredBooksDto;
        }
    }
}
