using System;

namespace BlogML2Ghost.Core.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static long AsMillisecondsSinceEpoch(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }

    
}