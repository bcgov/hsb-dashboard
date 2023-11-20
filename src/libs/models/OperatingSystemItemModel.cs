using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class OperatingSystemItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public JsonDocument? RawData { get; set; }
    public string ServiceNowKey { get; set; } = "";
    #endregion

    #region Constructors
    public OperatingSystemItemModel() { }

    public OperatingSystemItemModel(OperatingSystemItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.RawData = entity.RawData;
        this.ServiceNowKey = entity.ServiceNowKey;
    }
    #endregion

    #region Methods
    public OperatingSystemItem ToEntity()
    {
        if (this.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new OperatingSystemItem(this.RawData)
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
