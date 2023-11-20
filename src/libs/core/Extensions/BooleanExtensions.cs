namespace HSB.Core.Extensions;

/// <summary>
///
/// </summary>
public static class BooleanExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool TryParseBoolean(this string value, bool defaultValue = default)
    {
        return Boolean.TryParse(value, out bool result) ? result : defaultValue;
    }
}
