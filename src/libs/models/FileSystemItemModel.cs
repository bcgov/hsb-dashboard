using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class FileSystemItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public int ConfigurationItemId { get; set; }
    public JsonDocument? RawData { get; set; }
    public string ServiceNowKey { get; set; } = "";
    #endregion

    #region Constructors
    public FileSystemItemModel() { }

    public FileSystemItemModel(FileSystemItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.ConfigurationItemId = entity.ConfigurationItemId;
        this.RawData = entity.RawData;
        this.ServiceNowKey = entity.ServiceNowKey;
    }
    #endregion

    #region Methods
    public FileSystemItem ToEntity()
    {
        if (this.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new FileSystemItem(this.ConfigurationItemId, this.RawData)
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
