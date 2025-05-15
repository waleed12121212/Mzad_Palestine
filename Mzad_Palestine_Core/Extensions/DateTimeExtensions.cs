using System;

namespace Mzad_Palestine_Core.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo PalestineTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jerusalem");

        public static DateTime ToPalestineTime(this DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, PalestineTimeZone);
        }

        public static DateTime ToUtcFromPalestine(this DateTime palestineDateTime)
        {
            if (palestineDateTime.Kind != DateTimeKind.Unspecified)
            {
                palestineDateTime = DateTime.SpecifyKind(palestineDateTime, DateTimeKind.Unspecified);
            }
            return TimeZoneInfo.ConvertTimeToUtc(palestineDateTime, PalestineTimeZone);
        }
    }
} 