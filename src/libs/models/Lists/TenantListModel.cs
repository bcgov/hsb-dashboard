using HSB.Entities;

namespace HSB.Models;
public class TenantListModel : SortableCodeAuditableModel<int>
{
    #region Properties
    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - An array of organizations.
    /// </summary>
    public IEnumerable<OrganizationListModel> Organizations { get; set; } = Array.Empty<OrganizationListModel>();
    #endregion

    #region Constructors
    public TenantListModel() { }

    public TenantListModel(Tenant entity, bool includeOrganizations) : base(entity)
    {
        this.Id = entity.Id;

        this.ServiceNowKey = entity.ServiceNowKey;

        if (includeOrganizations)
        {
            this.Organizations = entity.OrganizationsManyToMany.Any() ? entity.OrganizationsManyToMany.Where(o => o.Organization != null).Select(o => new OrganizationListModel(o.Organization!, false)) : this.Organizations;
            this.Organizations = entity.Organizations.Any() ? entity.Organizations.Select(o => new OrganizationListModel(o, false)) : this.Organizations;
        }
    }

    public TenantListModel(ServiceNow.ResultModel<ServiceNow.TenantModel> model, IEnumerable<OrganizationListModel> organizations)
    {
        if (model.Data == null) throw new InvalidOperationException("Tenant data cannot be null");

        this.Name = model.Data.Name ?? model.Data.SysName ?? "";
        this.Code = model.Data.SysName ?? Guid.NewGuid().ToString();
        this.ServiceNowKey = model.Data.Id;

        this.Organizations = organizations;
    }
    #endregion
}
