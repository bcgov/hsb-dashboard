using System.Text.Json;
using HSB.Entities;
using HSB.Core.Extensions;

namespace HSB.Models;
public class TenantModel : SortableCodeAuditableModel<int>
{
    #region Properties
    public JsonDocument? RawData { get; set; }

    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - An array of organizations.
    /// </summary>
    public IEnumerable<OrganizationModel> Organizations { get; set; } = Array.Empty<OrganizationModel>();
    #endregion

    #region Constructors
    public TenantModel() { }

    public TenantModel(Tenant entity) : base(entity)
    {
        this.Id = entity.Id;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.RawData = entity.RawData;

        this.Organizations = entity.OrganizationsManyToMany.Where(o => o.Organization != null).Select(o => new OrganizationModel(o.Organization!));
        if (entity.Organizations.Any())
            this.Organizations = entity.Organizations.Select(o => new OrganizationModel(o));
    }

    public TenantModel(ServiceNow.ResultModel<ServiceNow.TenantModel> model, IEnumerable<OrganizationModel> organizations)
    {
        if (model.Data == null) throw new InvalidOperationException("Tenant data cannot be null");

        this.Name = model.Data.Name ?? model.Data.SysName ?? "";
        this.Code = model.Data.SysName ?? Guid.NewGuid().ToString();
        this.ServiceNowKey = model.Data.Id;
        this.RawData = model.RawData;

        this.Organizations = organizations;
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

        var tenant = new Tenant(model.Name)
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

        tenant.OrganizationsManyToMany.AddRange(model.Organizations.Select(o => new TenantOrganization(model.Id, o.Id)));

        return tenant;
    }
    #endregion
}
