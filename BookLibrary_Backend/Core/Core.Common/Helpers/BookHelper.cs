using System.Text.RegularExpressions;

namespace Core.Common.Helpers
{
    public static class BookHelper
    {
        /// <summary>
        /// If ISBN length is 13 char, It will insert a dash between char 3 and 4.
        /// </summary>
        /// <param name="isbn">a string that represent 13/14 character Book ISBN.</param>
        /// <returns>A 14 characters ISBN.</returns>
        public static string AddDashToIsbn(string isbn)
        {
            return isbn.Length == 14
                ? isbn
                : isbn.Insert(3, "-");
        }

        /// <summary>
        /// Verify if an isbn string has correct 13 or 14 (with dash) format.
        /// </summary>
        /// <param name="isbn">a string that represent 13/14 character Book ISBN.</param>
        /// <returns>True if isbn was in right format.</returns>
        public static bool VerifyIsbn(string isbn)
        {
            var isbnPattern = @"^\d{3}-{0,1}\d{10}$";
            return !string.IsNullOrEmpty(isbn) &&
                   Regex.IsMatch(isbn, isbnPattern);
        }
    }
}
