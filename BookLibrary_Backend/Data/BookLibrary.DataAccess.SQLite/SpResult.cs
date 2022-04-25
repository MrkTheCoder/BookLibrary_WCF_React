using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DataAccess.SQLite
{
    /// <summary>
    /// This is NotMapped Entity to use in:
    ///     CreateInitialDatabase.IsTableExists(BookLibraryDbContext ctx, string tableName)
    /// method to see if a specific Table exists or not.
    /// </summary>
    [NotMapped]
    public class SpResult
    {
        public int TableCount { get; set; }
    }
}