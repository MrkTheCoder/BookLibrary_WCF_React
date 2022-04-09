using System.Collections.Generic;
using BookLibrary.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite.Seeds
{
    public static class DbVersionSeed
    {
        public static void Data(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbVersion>()
                .HasData(new List<DbVersion> { new DbVersion { Id = 1,  Version = BookLibraryDbContext.DbVer } });
        }
    }
}
