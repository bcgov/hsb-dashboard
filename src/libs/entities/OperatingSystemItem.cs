using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class OperatingSystemItem : Auditable
{
    #region Properties
    public int Id { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    public List<ServerItem> ServerItems { get; } = new List<ServerItem>();

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    protected OperatingSystemItem() { }

    public OperatingSystemItem(JsonDocument data)
    {
        this.RawData = data;

        this.ServiceNowKey = data.GetElementValue<string>(".sys_id") ?? "";
        this.Name = data.GetElementValue<string>(".u_name") ?? "";
    }
    #endregion
}
