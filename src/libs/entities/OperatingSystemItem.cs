using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class OperatingSystemItem : Auditable
{
    #region Properties
    public int Id { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string UName { get; set; } = "";
    #endregion

    /// <summary>
    ///
    /// </summary>
    public ICollection<ServerItem> ServerItems { get; } = new List<ServerItem>();
    #endregion

    #region Constructors
    protected OperatingSystemItem() { }

    public OperatingSystemItem(JsonDocument serviceNowJson)
    {
        this.RawData = serviceNowJson;

        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(serviceNowJson);
        InitServiceNowProperties(values);
    }
    #endregion

    #region Methods
    private void InitServiceNowProperties(Dictionary<string, object>? values)
    {
        this.ServiceNowKey = values?.GetDictionaryJsonValue<string>("sys_id") ?? "";
        this.UName = values?.GetDictionaryJsonValue<string>("u_name") ?? "";
    }
    #endregion
}
