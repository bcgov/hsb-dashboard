using HSB.Entities;
using HSB.Core.Extensions;

namespace HSB.Models.Lists;
public class OperatingSystemItemListModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string ClassName { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    public OperatingSystemItemListModel() { }

    public OperatingSystemItemListModel(OperatingSystemItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.ServiceNowKey = entity.ServiceNowKey;
        this.Name = entity.Name;
        this.ClassName = entity.RawData.GetElementValue<string>(".u_class") ?? "";
    }

    public OperatingSystemItemListModel(ServiceNow.ResultModel<ServiceNow.OperatingSystemModel> model)
    {
        if (model.Data == null) throw new InvalidOperationException("Operating System data cannot be null");

        this.ServiceNowKey = model.Data.Id;
        this.Name = model.Data.Name ?? "";
        this.ClassName = model.Data.Class ?? "";
    }
    #endregion
}
