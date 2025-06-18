using System;

namespace HaMa.Scripts._Core.Utils
{
    public static class TimeUtils
    {
        public static string ToUtcFormat(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}