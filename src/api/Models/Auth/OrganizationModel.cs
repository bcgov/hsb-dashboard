
namespace HSB.API.Models.Auth;

/// <summary>
/// OrganizationModel class, provides a model to serialize organization data.
/// </summary>
public class OrganizationModel
{
    #region Properties
    /// <summary>
    /// get/set - Primary key to organization.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - The name of the organization.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// get/set - The unique code of the organization.
    /// </summary>
    public string Code { get; set; } = "";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an OrganizationModel object.
    /// </summary>
    public OrganizationModel() { }


    /// <summary>
    /// Creates a new instance of an OrganizationModel object, initializes with specified parameters.
    /// </summary>
    /// <param name="entity"></param>
    public OrganizationModel(Entities.Organization entity)
    {
        this.Id = entity.Id;
        this.Name = entity.Name;
        this.Code = entity.Code;
    }
    #endregion
}
