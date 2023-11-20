
namespace HSB.Core.Extensions;

using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
///
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static JsonSerializerOptions CreateJsonSerializerOptions(this IConfiguration configuration)
    {
        return configuration.CreateJsonSerializerOptions(new JsonSerializerOptions());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static JsonSerializerOptions CreateJsonSerializerOptions(this IConfiguration configuration, JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = configuration["Serialization:Json:DefaultIgnoreCondition"]?.TryParseEnum<JsonIgnoreCondition>() ?? JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNameCaseInsensitive = configuration["Serialization:Json:PropertyNameCaseInsensitive"]?.TryParseBoolean(true) ?? true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.WriteIndented = configuration["Serialization:Json:WriteIndented"]?.TryParseBoolean(true) ?? true;
        return options;
    }
}
