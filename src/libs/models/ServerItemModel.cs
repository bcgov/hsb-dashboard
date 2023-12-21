using System.Text.Json;
using HSB.Entities;
using HSB.Core.Extensions;

namespace HSB.Models;
public class ServerItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public long? ConfigurationItemId { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public OperatingSystemItemModel? OperatingSystem { get; set; }
    public JsonDocument? RawData { get; set; }

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string OperatingSystemKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string DiskSpace { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string Platform { get; set; } = "";
    public string IPAddress { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    public ServerItemModel() { }

    public ServerItemModel(ServerItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.ConfigurationItemId = entity.ConfigurationItemId;
        this.OperatingSystemItemId = entity.OperatingSystemItemId;
        this.RawData = entity.RawData;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.OperatingSystemKey = entity.OperatingSystemKey;
        this.Name = entity.Name;
        this.Category = entity.Category;
        this.SubCategory = entity.SubCategory;
        this.DiskSpace = entity.DiskSpace;
        this.DnsDomain = entity.DnsDomain;
        this.ClassName = entity.ClassName;
        this.Platform = entity.Platform;
        this.IPAddress = entity.IPAddress;
    }

    public ServerItemModel(ServiceNow.ResultModel<ServiceNow.ServerModel> model, long? configurationItemId, int? operatingSystemItemId)
    {
        if (model.Data == null) throw new InvalidOperationException("Server data cannot be null");

        this.ConfigurationItemId = configurationItemId;
        this.OperatingSystemItemId = operatingSystemItemId;
        this.RawData = model.RawData;

        this.ServiceNowKey = model.Data.Id;
        this.OperatingSystemKey = model.RawData.GetElementValue<string>(".u_operating_system.value") ?? model.RawData.GetElementValue<string>(".u_operating_system") ?? "";
        this.Name = model.Data.Name ?? "";
        this.Category = model.Data.Category ?? "";
        this.SubCategory = model.Data.SubCategory ?? "";
        this.DiskSpace = model.Data.DiskSpace ?? "";
        this.DnsDomain = model.Data.DnsDomain ?? "";
        this.ClassName = model.Data.ClassName ?? "";
        this.Platform = model.Data.Platform ?? "";
        this.IPAddress = model.Data.IPAddress ?? "";
    }
    #endregion

    #region Methods
    public ServerItem ToEntity()
    {
        return (ServerItem)this;
    }

    public static explicit operator ServerItem(ServerItemModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new ServerItem(model.ConfigurationItemId, model.OperatingSystemItemId, model.RawData)
        {
            Id = model.Id,
            CreatedOn = model.CreatedOn,
            CreatedBy = model.CreatedBy,
            UpdatedOn = model.UpdatedOn,
            UpdatedBy = model.UpdatedBy,
            Version = model.Version,
        };
    }
    #endregion
}
