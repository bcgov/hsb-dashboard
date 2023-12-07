using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class TenantModel : SortableCodeAuditableModel<int>
{
    #region Properties
    public JsonDocument? RawData { get; set; }

    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";
    #endregion

    #region Constructors
    public TenantModel() { }

    public TenantModel(Tenant tenant) : base(tenant)
    {
        this.Id = tenant.Id;
        this.RawData = tenant.RawData;
    }
    #endregion

    #region Methods
    public Tenant ToEntity()
    {
        return (Tenant)this;
    }

    public static explicit operator Tenant(TenantModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new Tenant(model.Name)
        {
            Id = model.Id,
            Description = model.Description,
            Code = model.Code,
            ServiceNowKey = model.ServiceNowKey,
            RawData = model.RawData,
            IsEnabled = model.IsEnabled,
            SortOrder = model.SortOrder,
            Version = model.Version,
        };
    }
    #endregion
}
