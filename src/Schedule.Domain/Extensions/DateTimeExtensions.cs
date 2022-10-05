using System.Globalization;

namespace Schedule.Domain;

public static class DateTimeExtensions
{
    const string DATE_FORMAT = "dd/MM/yyyy";
    static readonly DateTimeFormatInfo DTFI = new DateTimeFormatInfo() { DateSeparator = "/" };

    public static int ToUnixTimestamp(this DateTime datetime)
    {
        return (int)(datetime.Subtract(DateTime.UnixEpoch).TotalSeconds);
    }

    public static (DateTime, DateTime) ToMonthDateTimeRange(this DateTime seed)
    {
        var lower = new DateTime(seed.Year, seed.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1);
        var upper = new DateTime(seed.Year, seed.Month + 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return (lower, upper);
    }

    public static (int, int) ToMonthUnixTimestampRange(this DateTime seed)
    {
        var (lower, upper) = seed.ToMonthDateTimeRange();

        return (lower.ToUnixTimestamp(), upper.ToUnixTimestamp());
    }

    public static string ToFormattedDate(this int timestamp)
    {
        var datetime = DateTime.UnixEpoch.AddSeconds(timestamp);
        var result = datetime.ToString(DATE_FORMAT, DTFI);
        return result;
    }
}