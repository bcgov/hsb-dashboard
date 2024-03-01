using HSB.Entities;

namespace HSB.Models;
public class OrganizationListModel : SortableCodeAuditableModel<int>
{
    #region Properties
    public int? ParentId { get; set; }
    public OrganizationListModel? Parent { get; set; }

    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";
    public IEnumerable<OrganizationListModel> Children { get; set; } = Array.Empty<OrganizationListModel>();

    public IEnumerable<TenantListModel> Tenants { get; set; } = Array.Empty<TenantListModel>();
    #endregion

    #region Constructors
    public OrganizationListModel() { }

    public OrganizationListModel(Organization entity, bool includeTenants, bool includeChildren = false) : base(entity)
    {
        this.Id = entity.Id;
        this.ParentId = entity.ParentId;
        if (!includeChildren) this.Parent = entity.Parent != null ? new OrganizationListModel(entity.Parent, false) : null;
        if (includeChildren) this.Children = entity.Children.Select(c => new OrganizationListModel(c, false));

        if (includeTenants)
        {
            this.Tenants = entity.TenantsManyToMany.Any() ? entity.TenantsManyToMany.Where(t => t.Tenant != null).Select(t => new TenantListModel(t.Tenant!, false)).ToArray() : this.Tenants;
            this.Tenants = entity.Tenants.Any() ? entity.Tenants.Select(t => new TenantListModel(t, false)).ToArray() : this.Tenants;
        }

        this.ServiceNowKey = entity.ServiceNowKey;
    }

    public OrganizationListModel(ServiceNow.ResultModel<ServiceNow.ClientOrganizationModel> model)
    {
        if (model.Data == null) throw new InvalidOperationException("Organization data cannot be null");

        this.Name = model.Data.Name ?? "";
        this.Code = model.Data.OrganizationCode ?? Guid.NewGuid().ToString();
        this.ServiceNowKey = model.Data.Id;
    }
    #endregion
}
