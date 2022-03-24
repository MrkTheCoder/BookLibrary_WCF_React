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
                    new Book { Id = 8, Isbn = "101-88645218900", Title = "WPF in C# v10" },
                    new Book { Id = 9, Isbn = "101-88645448900", Title = "React in 7 Days" },
                    new Book { Id = 10, Isbn = "122-88655218900", Title = "Learn HTML with Samples" },
                    new Book { Id = 11, Isbn = "123-84445218900", Title = "Do Best Practice Of That!" },
                    new Book { Id = 12, Isbn = "132-38645218900", Title = "Server Client Architecture" },
                    new Book { Id = 13, Isbn = "322-88645245745", Title = "Design Patterns for Everyone" },
                    new Book { Id = 14, Isbn = "421-57434518900", Title = "SOLID in 24hours" },
                    new Book { Id = 15, Isbn = "333-88642458900", Title = "Can you do SOLID?" },
                    new Book { Id = 16, Isbn = "452-88345218900", Title = "Data Structure for Beginners" },
                    new Book { Id = 17, Isbn = "458-24672218900", Title = "Become Ready for Hard Interviews" },
                    new Book { Id = 18, Isbn = "421-88645622900", Title = "Get Pass all Interviews with One Tip" },
                    new Book { Id = 19, Isbn = "487-88645218923", Title = "Do you Really Need an Interview" },
                    new Book { Id = 20, Isbn = "258-88645213500", Title = "Get the job done by yourself" },
                    new Book { Id = 21, Isbn = "987-88645218921", Title = "Be your own boss" }
                );
        }
    }
}
