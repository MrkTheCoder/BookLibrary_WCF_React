using BookLibrary.Business.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Core.Common.Interfaces.Entities;
using Microsoft.EntityFrameworkCore.Design;

namespace BookLibrary.DataAccess.SQLite
{
    /// <summary>
    /// If Database is old version or not exists, It will be recreated.
    /// Call it like this:
    ///     CreateInitialDatabase.Initialize();
    /// </summary>
    public static class CreateInitialDatabase
    {
        private static bool _isInitialized = false;
        private static readonly object PadLock = new object();

        // Tell C# compiler not to mark type as beforefieldinit
        // (https://csharpindepth.com/articles/BeforeFieldInit)
        static CreateInitialDatabase() { }

        public static void Initialize(bool isRebuildDatabase = false)
        {
            // Double Check Lock Pattern
            if (_isInitialized && !isRebuildDatabase) return;

            lock (PadLock)
            {
                if (_isInitialized && !isRebuildDatabase) return;

                try
                {
                    SQLitePCL.Batteries.Init();

                    try
                    {
                        if (isRebuildDatabase)
                            RebuildDatabase();
                        else
                            IsDatabaseLatestVersion();
                    }
                    catch (SqliteException ex)
                    {
                        if (ex.Message.Contains("no such table: DbVersion") || ex.Message == "Old version!")
                        {
                            RebuildDatabase();
                            IsDatabaseLatestVersion();
                        }
                        else
                            Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception Name: {ex.GetType().Name}{Environment.NewLine}" +
                                          $"Message:        {ex.Message}");
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _isInitialized = true;
                }
            }
        }

        private static void RebuildDatabase()
        {
            using (var ctx = new BookLibraryDbContext())
            {
                ctx.Database.EnsureDeleted();
                if (ctx.Database.EnsureCreated())
                    AddTriggers(ctx);
            }
        }

        private static void AddTriggers(BookLibraryDbContext ctx)
        {
            var type = typeof(IEntityBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && p.GetProperty("RowVersion") != null);

            foreach (var entity in types)
            {
                var swlCommand = $@"CREATE TRIGGER UpdateBookVersion
                AFTER UPDATE ON {entity.Name}
                BEGIN
                    UPDATE {entity.Name}
                    SET RowVersion = RowVersion + 1
                    WHERE rowid = NEW.rowid;
                END;";
                ctx.Database.ExecuteSqlRaw(swlCommand); //new SqlParameter("@Entities", nameof(Book)));
            }
        }

        private static void IsDatabaseLatestVersion()
        {
            using (var ctx = new BookLibraryDbContext())
            {
                if (!IsTableExists(ctx, nameof(DbVersion)))
                    throw new SqliteException("Old version!", 1);


                var version = ctx.DbVersion.First().Version;

                if (version != BookLibraryDbContext.DbVer)
                    throw new SqliteException("Old version!", 1);
            }
        }

        private static bool IsTableExists(BookLibraryDbContext ctx, string tableName)
        {
            // https://stackoverflow.com/questions/68601822/ef-core-sqlite-checking-if-table-exists
            var sql = $"SELECT COUNT(*) AS TableCount FROM sqlite_master WHERE type = 'table' AND name = '{tableName}';";

            var result = ctx.Set<SpResult>()
                .FromSqlRaw(sql)
                .ToArray();

            var spResult = result.FirstOrDefault();

            return spResult != null && spResult.TableCount > 0;
        }
    }
}
