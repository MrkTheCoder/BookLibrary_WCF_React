using System;

namespace Core.Common.Helpers
{
    public class DateTimeHelper
    {
        public static byte[] GetTimestamp(int increaseByDay = 0)
        {
            var timestamp = BitConverter.GetBytes(DateTime.Now.AddDays(increaseByDay).Ticks);
            return timestamp;
        }
    }
}
