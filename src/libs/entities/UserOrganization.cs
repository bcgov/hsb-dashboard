namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class UserOrganization : Auditable
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
    /// <param name="user"></param>
    /// <param name="organization"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserOrganization(User user, Organization organization)
    {
        this.User = user ?? throw new ArgumentNullException(nameof(user));
        this.UserId = user.Id;
        this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
        this.OrganizationId = organization.Id;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="organizationId"></param>
    public UserOrganization(int userId, int organizationId)
    {
        this.UserId = userId;
        this.OrganizationId = organizationId;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not UserOrganization entity) return false;
        return (this.UserId, this.OrganizationId).Equals((entity.UserId, entity.OrganizationId));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, OrganizationId);
    }
    #endregion
}
