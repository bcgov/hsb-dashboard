namespace HSB.Core;

public static class Converter
{
    /// <summary>
    /// Convert the specified 'value' to the specified 'type'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? ChangeType<T>(object value)
    {
        var t = typeof(T);

        if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
                return default;

            t = Nullable.GetUnderlyingType(t);
            if (t == null)
                return default;
        }

        return (T)Convert.ChangeType(value, t);
    }

    /// <summary>
    /// Convert the specified 'value' to the specified 'type'.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="conversion"></param>
    /// <returns></returns>
    public static object? ChangeType(object value, Type conversion)
    {
        var t = conversion;

        if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
                return null;

            t = Nullable.GetUnderlyingType(t);
            if (t == null)
                return default;
        }

        return Convert.ChangeType(value, t);
    }
}
