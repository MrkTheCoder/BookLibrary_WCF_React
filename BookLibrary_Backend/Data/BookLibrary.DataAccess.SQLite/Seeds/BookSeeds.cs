using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class BookSeeds
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Book>()
                .HasData
                (
                    new Book { Id = 1, Isbn = "101-11113502222", Title = "Clean Code: Best Practices" },
                    new Book { Id = 2, Isbn = "101-22258489522", Title = "Learn Design Patterns in R" },
                    new Book { Id = 3, Isbn = "131-33333889820", Title = "A Red Apple Far From Tree" },
                    new Book { Id = 4, Isbn = "141-44444886529", Title = "How to Fix Broken Chair" },
                    new Book { Id = 5, Isbn = "131-55554825527", Title = "Do We Need Another One?" },
                    new Book { Id = 6, Isbn = "131-66684885431", Title = "Hello World!" },
                    new Book { Id = 7, Isbn = "121-77784885431", Title = "Java va JavaScript" },
                    new Book { Id = 8, Isbn = "101-88645218900", Title = "WPF in C# v10" }
                );
        }
    }
}
