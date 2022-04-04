using System;
using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class BorrowerSeeds
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            var generalLink = "https://drive.google.com/uc?export=view&id=";

            var femaleLink = generalLink + "1bQWpmL5keOb6J71lxZH2BK75FAdo1cSH";     // Id = 1
            var maleLink = generalLink + "1JxCdcwUUe3e7_p0XRWxrhpzYqyvfw-pH";       // Id = 2
            var transLink = generalLink + "1gb21iLean2oSEZIQrUz1dViw24ecaLlx";      // Id = 3
            var otherLink = generalLink + "11QF0vMALutKa9NI3F264x6FGx6Va2sZn";      // Id = 4
            var notAppLink = generalLink + "1MNY0dGnhs4djIc4hWxarMMhsRflv-K5h";     // Id = 5

            modelBuilder.Entity<Borrower>()
                .HasData
                    (
                        new Borrower {Id = 1, GenderId = 2, RegistrationDate = new DateTime(1998, 1, 16, 12, 2 ,15), FirstName = "John", LastName = "Smith", PhoneNo = "+15554441234", Username = "johns", Password = "johns123", AvatarLink = maleLink},
                        new Borrower {Id = 2, GenderId = 1, RegistrationDate = new DateTime(1999, 2, 10, 1, 2 ,5),FirstName = "Jane", LastName = "Smith", PhoneNo = "+15554445678", Username = "janes", Password = "janes123", AvatarLink = femaleLink},
                        new Borrower {Id = 3, GenderId = 3, RegistrationDate = new DateTime(2001, 9, 23, 17, 32 ,10),FirstName = "John", MiddleName = "Junior", LastName = "Due", PhoneNo = "+905554441234", Username = "johnjd", Password = "jjd123", AvatarLink = transLink},
                        new Borrower {Id = 4, GenderId = 1, RegistrationDate = new DateTime(2001, 9, 24, 12, 30 ,45),FirstName = "Jane", LastName = "Doe", PhoneNo = "+985554449834", Username = "janed", Password = "janed123", AvatarLink = femaleLink},
                        new Borrower {Id = 5, GenderId = 4, RegistrationDate = new DateTime(2003, 10, 6, 2, 26 ,55),FirstName = "John", LastName = "Mich", PhoneNo = "+265554441234", Username = "johnm", Password = "jm123", AvatarLink = otherLink},
                        new Borrower {Id = 6, GenderId = 5, RegistrationDate = new DateTime(2006, 11, 1, 8, 6 ,3),FirstName = "Nan", LastName = "Niamy", PhoneNo = "+335554441234", Username = "nann", Password = "nn123", AvatarLink = notAppLink},
                        new Borrower {Id = 7, GenderId = 4, RegistrationDate = new DateTime(2007, 7, 27, 19, 2 ,15),FirstName = "Amy", MiddleName = "3rd" , LastName = "Nolan", PhoneNo = "+495554441234", Username = "amy3n", Password = "a3n123", AvatarLink = otherLink},
                        new Borrower {Id = 8, GenderId = 1, RegistrationDate = new DateTime(2008, 4, 16, 12, 12 ,5),FirstName = "Anna", LastName = "Marshall", PhoneNo = "+655554441234", Username = "annam", Password = "an123", AvatarLink = femaleLink},
                        new Borrower {Id = 9, GenderId = 2, RegistrationDate = new DateTime(2018, 6, 16, 1, 33 ,9),FirstName = "Leonard", MiddleName = "Senior", LastName = "Robertson", PhoneNo = "+715554441234", Username = "leonards", Password = "ls123", AvatarLink = maleLink},
                        new Borrower {Id = 10, GenderId = 3, RegistrationDate = new DateTime(2020, 3, 16, 3, 27 ,37),FirstName = "Stewart", MiddleName = "Nancy", LastName = "Hughes", PhoneNo = "+45554441234", Username = "stewartnh", Password = "snh123", AvatarLink = transLink},
                        new Borrower {Id = 11, GenderId = 5, RegistrationDate = new DateTime(2022, 1, 16, 19, 53 ,41),FirstName = "Emma", LastName = "Newman", PhoneNo = "+985554441234", Username = "emman", Password = "en123", AvatarLink = notAppLink}
                    );
        }
    }
}
