using System;
using BookLibrary.Business.Entities;
using Core.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class BookSeeds
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            var thumbnailImage = "https://drive.google.com/uc?export=view&id=1_MdrrFLXKPJ6e02kLANMEC8JegfFzuE1";
            var originalImage = "https://drive.google.com/uc?export=view&id=1lWnDAiITllbJ3bAB9I7Thfw0oueyN6SW";
            modelBuilder
                .Entity<Book>()
                .HasData
                (
                    new Book { Id = 1, Isbn = "101-1111350222", Title = "Clean Code: Best Practices", BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 2, Isbn = "101-2225848952", Title = "Learn Design Patterns in R" , BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 3, Isbn = "131-3333388982", Title = "A Red Apple Far From Tree" , BookCategoryId = 6, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 4, Isbn = "141-4444488652", Title = "How to Fix Broken Chair" , BookCategoryId = 1, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 5, Isbn = "131-5555482552", Title = "Do We Need Another One?" , BookCategoryId = 6, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 6, Isbn = "131-6668488543", Title = "Hello World!" , BookCategoryId = 3, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 7, Isbn = "121-7778488543", Title = "Java vs JavaScript" , BookCategoryId = 3, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 8, Isbn = "101-8864521890", Title = "WPF in C# v10" , BookCategoryId = 3, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 9, Isbn = "101-8864544890", Title = "React in 7 Days" , BookCategoryId = 4, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 10, Isbn = "122-8865521890", Title = "Learn HTML with Samples" , BookCategoryId = 4, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 11, Isbn = "123-8444521890", Title = "Do Best Practice Of That!" , BookCategoryId = 6, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 12, Isbn = "132-3864521890", Title = "Server Client Architecture" , BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 13, Isbn = "322-8864524575", Title = "Design Patterns for Everyone" , BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 14, Isbn = "421-5743451890", Title = "SOLID in 24hours" , BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 15, Isbn = "333-8864245890", Title = "Can you do SOLID?" , BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 16, Isbn = "452-8834521890", Title = "Data Structure for Beginners" , BookCategoryId = 2, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 17, Isbn = "458-2467221890", Title = "Become Ready for Hard Interviews" , BookCategoryId = 5, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 18, Isbn = "421-8864562290", Title = "Get Pass all Interviews with One Tip" , BookCategoryId = 5, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 19, Isbn = "487-8864521893", Title = "Do you Really Need an Interview" , BookCategoryId = 5, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 20, Isbn = "258-8864521350", Title = "Get the job done by yourself" , BookCategoryId = 6, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage},
                    new Book { Id = 21, Isbn = "987-8864521891", Title = "Be your own boss" , BookCategoryId = 6, CoverLinkThumbnail = thumbnailImage, CoverLinkOriginal = originalImage}
                );
        }

    }
}
