using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Models.ServiceNow;

/// <summary>
/// ResultModel class, provides a model to hold Service Now data results.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResultModel<T>
{
    #region Properties
    /// <summary>
    /// get/set - The deserialized data from Service Now.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// get/set - The raw data from Service Now.
    /// </summary>
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ResultModel object, initializes with specified parameters.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="rawData"></param>
    public ResultModel(T? data, JsonDocument rawData)
    {
        this.Data = data;
        this.RawData = rawData.GetElementValue<JsonDocument>(".result") ?? rawData;
    }
    #endregion
}
