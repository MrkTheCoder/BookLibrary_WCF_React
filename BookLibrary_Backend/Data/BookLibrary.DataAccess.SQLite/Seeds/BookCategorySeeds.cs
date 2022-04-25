using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class BookCategorySeeds
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BookCategory>()
                .HasData
                    (
                        new BookCategory{ Id = 1, Name = "DIY"},
                        new BookCategory{ Id = 2, Name = "Architecture"},
                        new BookCategory{ Id = 3, Name = "Coding"},
                        new BookCategory{ Id = 4, Name = "Web"},
                        new BookCategory{ Id = 5, Name = "Interview"},
                        new BookCategory{ Id = 6, Name = "Story"},
                        new BookCategory{ Id = 7, Name = "Space"}
                    );
        }
    }
}
