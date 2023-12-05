using HSB.Entities;

namespace HSB.Models;
public class RoleModel : SortableAuditableModel<int>
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<GroupModel> Groups { get; set; } = Array.Empty<GroupModel>();
    #endregion

    #region Constructors
    public RoleModel() { }

    public RoleModel(Role role) : base(role)
    {
        this.Id = role.Id;
        this.Name = role.Name;
    }
    #endregion

    #region Methods
    public Role ToEntity()
    {
        return (Role)this;
    }

    public static explicit operator Role(RoleModel model)
    {
        var role = new Role(model.Name, model.Key)
        {
            Id = model.Id,
            Description = model.Description,
            SortOrder = model.SortOrder,
            IsEnabled = model.IsEnabled,
            Version = model.Version,
        };
        role.GroupsManyToMany.AddRange(model.Groups.Select(g => new GroupRole(g.Id, role.Id)));

        return role;
    }
    #endregion
}
