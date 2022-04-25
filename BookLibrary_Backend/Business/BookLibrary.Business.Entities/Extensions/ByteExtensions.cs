using System.Linq;

namespace BookLibrary.Business.Entities.Extensions
{
    public static class ByteExtensions
    {
        public static string ToHexString(this byte[] byteArray) =>
            string.Join(string.Empty, byteArray.Select(b => b.ToString("X")));
    }
}
