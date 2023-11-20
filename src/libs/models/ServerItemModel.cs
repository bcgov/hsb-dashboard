using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class ServerItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public int ConfigurationItemId { get; set; }
    public int OperatingSystemId { get; set; }
    public OperatingSystemItemModel? OperatingSystem { get; set; }
    public JsonDocument? RawData { get; set; }
    public string ServiceNowKey { get; set; } = "";
    #endregion

    #region Constructors
    public ServerItemModel() { }

    public ServerItemModel(ServerItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.ConfigurationItemId = entity.ConfigurationItemId;
        this.OperatingSystemId = entity.OperatingSystemItemId;
        this.OperatingSystem = entity.OperatingSystemItem != null ? new OperatingSystemItemModel(entity.OperatingSystemItem) : null;
        this.RawData = entity.RawData;
        this.ServiceNowKey = entity.ServiceNowKey;
    }
    #endregion

    #region Methods
    public ServerItem ToEntity()
    {
        if (this.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new ServerItem(this.ConfigurationItemId, this.OperatingSystemId, this.RawData)
        {
            Id = this.Id,
            CreatedOn = this.CreatedOn,
            CreatedBy = this.CreatedBy,
            UpdatedOn = this.UpdatedOn,
            UpdatedBy = this.UpdatedBy,
            Version = this.Version,
        };
    }
    #endregion
}
