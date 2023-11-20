namespace HSB.Entities;

public enum OrganizationType
{
    /// <summary>
    /// An organization is a government agency.
    /// </summary>
    Organization = 0,
    /// <summary>
    /// A ministry is a parent to organizations.
    /// </summary>
    Ministry = 1,
    /// <summary>
    /// A sector is a parent to ministries.
    /// </summary>
    Sector = 2
}
