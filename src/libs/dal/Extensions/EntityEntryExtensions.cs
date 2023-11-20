using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HSB.DAL.Extensions;

/// <summary>
/// EntityEntryExtensions static class, provides extension methods for EntityEntry objects.
/// </summary>
public static class EntityEntryExtensions
{
    /// <summary>
    /// Extracts the original value for the specified property 'name'.  If it's null it will return the default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entry"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetOriginalValue<T>(this EntityEntry entry, string name, T defaultValue)
    {
        var value = entry.OriginalValues[name];

        if (value == null) return defaultValue;

        return (T)value;
    }
}
