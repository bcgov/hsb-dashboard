namespace HSB.Core.Extensions;

/// <summary>
/// DateTimeExtensions static class, provides extension methods for DateTime objects.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Convert the specified timestamp into a DateTime object.
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime ConvertFromUnixTimestamp(this double timestamp)
    {
        return DateTime.UnixEpoch.AddSeconds(timestamp);
    }

    /// <summary>
    /// Converts the specified date into a Unix Epoch time value.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static double ConvertToUnixTimestamp(this DateTime date)
    {
        return Math.Floor((date.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds);
    }

    /// <summary>
    /// Find the first day in the month that matches the specified 'dayOfWeek'.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static int GetFirstDayOfWeekInMonth(this DateTime date, DayOfWeek dayOfWeek)
    {
        var month = date.AddDays(1 - date.Day);
        while (month.DayOfWeek != dayOfWeek)
        {
            month = month.AddDays(1);
        }
        return month.Day;
    }
}
