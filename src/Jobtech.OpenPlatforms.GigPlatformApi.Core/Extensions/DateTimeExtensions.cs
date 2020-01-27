using System;
using System.Globalization;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIsoString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
