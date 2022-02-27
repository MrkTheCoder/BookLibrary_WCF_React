using System;
using System.IO;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.SQLite.Seeds;
using Core.Common.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DataAccess.SQLite
{
    public partial class BookLibraryDbContext : DbContext
    {
        private const string DatabaseFilename = "LocalDb.sqlite";
        private static bool HasCheckedDatabase { get; set; }

        public BookLibraryDbContext()
        {
            if (!HasCheckedDatabase)
                CheckDatabase();
        }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={GetDatabaseFile()}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignoring Interfaces and Entities Properties
            modelBuilder.Ignore<IIdentifiableEntity>();
            modelBuilder.Entity<Book>().Ignore(p => p.EntityId);


            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book", "BookSchema");

                entity.Property(e => e.Isbn)
                    .HasColumnName("ISBN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);

            SeedingDatabase(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void SeedingDatabase(ModelBuilder modelBuilder)
        {
            BookSeeds.Data(modelBuilder);
        }

        private string GetDatabaseFile()
        {
            var installationDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(installationDirectory, DatabaseFilename);

            return filePath;
        }

        private void CheckDatabase()
        {
            try
            {
                SQLitePCL.Batteries_V2.Init();
                Database.EnsureCreated();
                HasCheckedDatabase = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
