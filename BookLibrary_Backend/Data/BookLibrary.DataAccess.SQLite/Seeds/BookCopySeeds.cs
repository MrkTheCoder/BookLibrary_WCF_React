using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class BookCopySeeds
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BookCopy>()
                .HasData (BookCopyFeed());
        }

        private static BookCopy[] BookCopyFeed()
        {
            var bookCopies = new List<BookCopy>();
            
            for (int i = 1; i <= 21; i++)
            {
                bookCopies.Add(new BookCopy {BookId = i, TotalCopy = i});
            }

            return bookCopies.ToArray();
        }
    }
}
