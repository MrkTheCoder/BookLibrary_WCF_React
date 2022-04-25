using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class GenderSeeds
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>()
                .HasData
                    (
                        new Gender {Id = 1, Type = "Female"},
                        new Gender {Id = 2, Type = "Male"},
                        new Gender {Id = 3, Type = "Transgender"},
                        new Gender {Id = 4, Type = "Others"},
                        new Gender {Id = 5,Type = "Not Applicable"}
                    );
        }
    }
}
