using System.Text.Json;
using HSB.Entities;
using System.Text.Json.Serialization;

namespace HSB.Models.Dashboard;

public class CompactServerHistoryItemModel
{
    #region Properties
    public long Id { get; set; }
    public int? TenantId { get; set; }
    public int OrganizationId { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public float? Capacity { get; set; }
    public float? AvailableSpace { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }

    #endregion

    #region Constructors
    public CompactServerHistoryItemModel() { }

    public CompactServerHistoryItemModel(CompactServerHistoryItem entity)
    {
        this.Id = entity.Id;

        this.TenantId = entity.TenantId;
        this.OrganizationId = entity.OrganizationId;
        this.OperatingSystemItemId = entity.OperatingSystemItemId;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.Name = entity.Name;

        this.Capacity = entity.Capacity;
        this.AvailableSpace = entity.AvailableSpace;

        this.CreatedOn = entity.CreatedOn;
        this.UpdatedOn = entity.UpdatedOn;

    }

    public CompactServerHistoryItemModel(ServerHistoryItemSmall entity)
    {
        this.Id = entity.Id;

        this.TenantId = entity.TenantId;
        this.OrganizationId = entity.OrganizationId;
        this.OperatingSystemItemId = entity.OperatingSystemItemId;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.Name = entity.Name;

        this.Capacity = entity.Capacity;
        this.AvailableSpace = entity.AvailableSpace;

        this.CreatedOn = entity.CreatedOn;
        this.UpdatedOn = entity.UpdatedOn;
    }


    #endregion

}
