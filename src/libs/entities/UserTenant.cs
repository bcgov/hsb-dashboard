namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class UserTenant : Auditable
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Tenant? Tenant { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="tenant"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserTenant(User user, Tenant tenant)
    {
        this.User = user ?? throw new ArgumentNullException(nameof(user));
        this.UserId = user.Id;
        this.Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
        this.TenantId = tenant.Id;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tenantId"></param>
    public UserTenant(int userId, int tenantId)
    {
        this.UserId = userId;
        this.TenantId = tenantId;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not UserTenant entity) return false;
        return (this.UserId, this.TenantId).Equals((entity.UserId, entity.TenantId));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, TenantId);
    }
    #endregion
}
