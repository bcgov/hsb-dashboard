using HSB.Entities;

namespace HSB.Models;
public class LookupModel
{
    #region Properties
    public IEnumerable<TenantModel> Tenants { get; set; } = Array.Empty<TenantModel>();
    public IEnumerable<OrganizationModel> Organizations { get; set; } = Array.Empty<OrganizationModel>();
    public IEnumerable<OperatingSystemItemModel> OperatingSystemItems { get; set; } = Array.Empty<OperatingSystemItemModel>();
    public IEnumerable<ServerItemModel> ServerItems { get; set; } = Array.Empty<ServerItemModel>();
    #endregion

    #region Constructors
    public LookupModel() { }

    public LookupModel(IEnumerable<Tenant> tenants, IEnumerable<Organization> organizations, IEnumerable<OperatingSystemItem> operatingSystemItems, IEnumerable<ServerItem> serverItems)
    {
        this.Tenants = tenants.Select(t => new TenantModel(t, true)).ToArray();
        this.Organizations = organizations.Select(t => new OrganizationModel(t, true)).ToArray();
        this.OperatingSystemItems = operatingSystemItems.Select(t => new OperatingSystemItemModel(t)).ToArray();
        this.ServerItems = serverItems.Select(t => new ServerItemModel(t)).ToArray();
    }

    public LookupModel(IEnumerable<TenantModel> tenants, IEnumerable<OrganizationModel> organizations, IEnumerable<OperatingSystemItemModel> operatingSystemItems, IEnumerable<ServerItemModel> serverItems)
    {
        this.Tenants = tenants;
        this.Organizations = organizations;
        this.OperatingSystemItems = operatingSystemItems;
        this.ServerItems = serverItems;
    }
    #endregion
}
