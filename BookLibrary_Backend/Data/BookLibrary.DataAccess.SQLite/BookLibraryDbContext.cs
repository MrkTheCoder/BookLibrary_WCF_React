using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.SQLite.Seeds;
using Core.Common.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BookLibrary.DataAccess.SQLite
{
    public partial class BookLibraryDbContext : DbContext
    {
        public const string DbVer = "v0.6.13";
        private const string DatabaseFilename = "LocalDb.sqlite";
        private static string _databasePath = null;


        public BookLibraryDbContext()
        {
            if (_databasePath == null)
                _databasePath = GetDatabaseFile();
        }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            : base(options)
        {
        }

        // A table with one record for checking current Database version.
        public virtual DbSet<SpResult> SpResults { get; set; }
        public virtual DbSet<DbVersion> DbVersion { get; set; }
        // Main app entities
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCategory> BookCategories { get; set; }
        public virtual DbSet<BookCopy> BookCopies { get; set; }
        public virtual DbSet<Borrower> Borrowers { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={_databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignoring Interfaces and Entities Properties
            modelBuilder.Ignore<IEntityBase>();
            modelBuilder.Ignore<IIdentifiableEntity>();
            modelBuilder.Ignore<IEntityVersion>();
            modelBuilder.Entity<Book>().Ignore(p => p.EntityId);
            modelBuilder.Entity<BookCategory>().Ignore(p => p.EntityId);
            modelBuilder.Entity<BookCopy>().Ignore(p => p.EntityId);
            modelBuilder.Entity<Borrower>().Ignore(p => p.EntityId);
            modelBuilder.Entity<Gender>().Ignore(p => p.EntityId);

            // A dummy NotMapped Table to use here: CreateInitialDatabase.IsTableExists(BookLibraryDbContext ctx, string tableName)
            modelBuilder.Entity<SpResult>().HasNoKey();

            // Book Related tables
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book", "BookSchema");

                entity.HasIndex(e => e.Isbn)
                    .HasName("UK_Book_ISBN")
                    .IsUnique();

                entity.Property(e => e.CoverLinkOriginal).HasMaxLength(255);

                entity.Property(e => e.CoverLinkThumbnail).HasMaxLength(255);

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnName("ISBN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .HasDefaultValue(0);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BookCategory)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.BookCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Book_BookCategory_Id");
            });

            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.ToTable("BookCategory", "BookSchema");

                entity.HasIndex(e => e.Name)
                    .HasName("UK_BookCategory_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BookCopy>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.ToTable("BookCopy", "BookSchema");

                entity.Property(e => e.BookId).ValueGeneratedNever();

                entity.HasOne(d => d.Book)
                    .WithOne(p => p.BookCopy)
                    .HasForeignKey<BookCopy>(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookCopy_Book_Id");
            });

            // Borrower Related Tables

            modelBuilder.Entity<Borrower>(entity =>
            {
                entity.ToTable("Borrower", "BorrowerSchema");

                entity.HasIndex(e => e.Email)
                    .HasName("UK_Borrower_Email")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNo)
                    .HasName("UK_Borrower_PhoneNo")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("UK_Borrower_Username")
                    .IsUnique();

                entity.Property(e => e.AvatarLink)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Borrowers)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Borrower_Gender_Id");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender", "BorrowerSchema");

                entity.HasIndex(e => e.Type)
                    .HasName("IX_Gender")
                    .IsUnique();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            OnModelCreatingPartial(modelBuilder);

            SeedingDatabase(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        private void SeedingDatabase(ModelBuilder modelBuilder)
        {
            // Set current Database version.
            DbVersionSeed.Data(modelBuilder);
            // App Entities
            BookCategorySeeds.Data(modelBuilder);
            BookSeeds.Data(modelBuilder);
            BookCopySeeds.Data(modelBuilder);
            GenderSeeds.Data(modelBuilder);
            BorrowerSeeds.Data(modelBuilder);
        }

        private string GetDatabaseFile()
        {
            var installationDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(installationDirectory, DatabaseFilename);

            return filePath;
        }
    }
}
