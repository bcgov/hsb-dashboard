using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class ConfigurationItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public OrganizationModel? Organization { get; set; }
    public IEnumerable<ServerItemModel> Servers { get; set; } = Array.Empty<ServerItemModel>();
    public IEnumerable<FileSystemItemModel> FileSystems { get; set; } = Array.Empty<FileSystemItemModel>();
    public JsonDocument? RawData { get; set; }
    public string ServiceNowKey { get; set; } = "";
    #endregion

    #region Constructors
    public ConfigurationItemModel() { }

    public ConfigurationItemModel(ConfigurationItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.OrganizationId = entity.OrganizationId;
        this.Organization = entity.Organization != null ? new OrganizationModel(entity.Organization) : null;
        this.RawData = entity.RawData;
        this.ServiceNowKey = entity.ServiceNowKey;

        this.Servers = entity.ServerItems.Select(si => new ServerItemModel(si));
        this.FileSystems = entity.FileSystemItems.Select(fsi => new FileSystemItemModel(fsi));
    }
    #endregion

    #region Methods
    public ConfigurationItem ToEntity()
    {
        if (this.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new ConfigurationItem(this.OrganizationId, this.RawData)
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
