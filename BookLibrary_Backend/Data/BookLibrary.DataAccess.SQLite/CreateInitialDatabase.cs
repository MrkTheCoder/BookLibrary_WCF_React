using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace BookLibrary.DataAccess.SQLite
{
    public class CreateInitialDatabase
    {
        private static bool _isInitialized = false;
        private static readonly object PadLock = new object();

        // Tell C# compiler not to mark type as beforefieldinit
        // (https://csharpindepth.com/articles/BeforeFieldInit)
        static CreateInitialDatabase() { }

        private CreateInitialDatabase()
        { }

        public static void Initialize()
        {
            if (_isInitialized) return;
            lock (PadLock)
            {
                if (_isInitialized) return;

                // a flag to make sure not get in unlimited loops!
                var hasExceptions = false;
                try
                {
                    while (true)
                    {
                        try
                        {
                            using (var ctx = new BookLibraryDbContext())
                            {
                                var version = ctx.DbVersion.First().Version;

                                if (version != BookLibraryDbContext.DbVer)
                                    throw new SqliteException("Old version!", 1);

                                break;
                            }
                        }
                        catch (SqliteException ex)
                        {
                            if (!hasExceptions &&
                                (
                                    ex.Message == "SQLite Error 1: 'no such table: DbVersion'." ||
                                    ex.Message == "Old version!"
                                ))
                                using (var ctx = new BookLibraryDbContext())
                                {
                                    hasExceptions = true;
                                    ctx.Database.EnsureDeleted();
                                    ctx.Database.EnsureCreated();
                                    continue;
                                }

                            Console.WriteLine(ex.Message);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception Name: {ex.GetType().Name}{Environment.NewLine}" +
                                              $"Message:        {ex.Message}");
                            break;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

}
