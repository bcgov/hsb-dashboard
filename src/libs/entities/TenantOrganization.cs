namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class TenantOrganization : Auditable
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Tenant? Tenant { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Organization? Organization { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    ///
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="organization"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TenantOrganization(Tenant tenant, Organization organization)
    {
        this.Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
        this.TenantId = tenant.Id;
        this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
        this.OrganizationId = organization.Id;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="organizationId"></param>
    public TenantOrganization(int tenantId, int organizationId)
    {
        this.TenantId = tenantId;
        this.OrganizationId = organizationId;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not TenantOrganization entity) return false;
        return (this.TenantId, this.OrganizationId).Equals((entity.TenantId, entity.OrganizationId));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(TenantId, OrganizationId);
    }
    #endregion
}
