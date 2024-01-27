
namespace HSB.API.Models.Auth;

/// <summary>
/// TenantModel class, provides a model to serialize tenant data.
/// </summary>
public class TenantModel
{
    #region Properties
    /// <summary>
    /// get/set - Primary key to tenant.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - The name of the tenant.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// get/set - The unique code of the tenant.
    /// </summary>
    public string Code { get; set; } = "";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an TenantModel object.
    /// </summary>
    public TenantModel() { }


    /// <summary>
    /// Creates a new instance of an TenantModel object, initializes with specified parameters.
    /// </summary>
    /// <param name="entity"></param>
    public TenantModel(Entities.Tenant entity)
    {
        this.Id = entity.Id;
        this.Name = entity.Name;
        this.Code = entity.Code;
    }
    #endregion
}
