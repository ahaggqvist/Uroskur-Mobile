namespace Uroskur.Utils;

internal static class DateTimeHelper
{
    public static DateTime UnixTimestampToDateTime(double unixTime)
    {
        var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
        var unixTimestampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
        return new DateTime(unixStart.Ticks + unixTimestampInTicks, DateTimeKind.Local);
    }

    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
        var unixTimestampInTicks = (dateTime.ToLocalTime() - unixStart).Ticks;
        return unixTimestampInTicks / TimeSpan.TicksPerSecond;
    }
}