using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class OperatingSystemItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public JsonDocument? RawData { get; set; }

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    public OperatingSystemItemModel() { }

    public OperatingSystemItemModel(OperatingSystemItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.RawData = entity.RawData;
        this.ServiceNowKey = entity.ServiceNowKey;
        this.Name = entity.Name;
    }

    public OperatingSystemItemModel(ServiceNow.ResultModel<ServiceNow.OperatingSystemModel> model)
    {
        if (model.Data == null) throw new InvalidOperationException("Operating System data cannot be null");

        this.ServiceNowKey = model.Data.Id;
        this.Name = model.Data.Name ?? "";
        this.RawData = model.RawData;
    }
    #endregion

    #region Methods
    public OperatingSystemItem ToEntity()
    {
        return (OperatingSystemItem)this;
    }

    public static explicit operator OperatingSystemItem(OperatingSystemItemModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new OperatingSystemItem(model.RawData)
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
