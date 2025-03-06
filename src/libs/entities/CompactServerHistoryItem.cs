using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;

public class CompactServerHistoryItem
{
    #region Properties
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public int? TenantId { get; set; }
    public int OrganizationId { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public string ServiceNowKey { get; set; } = "";
    public float? Capacity { get; set; }
    public float? AvailableSpace { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
    #endregion

    #region Constructors
    public CompactServerHistoryItem() {}
    #endregion
}
