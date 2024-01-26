using HSB.Entities;

namespace HSB.Models;
public class GroupModel : SortableAuditableModel<int>
{
    #region Properties
    /// <summary>
    /// get/set -
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    public IEnumerable<RoleModel> Roles { get; set; } = Array.Empty<RoleModel>();

    /// <summary>
    /// get/set -
    /// </summary>
    public IEnumerable<UserModel> Users { get; set; } = Array.Empty<UserModel>();
    #endregion

    #region Constructors
    public GroupModel() { }

    public GroupModel(Group group) : base(group)
    {
        this.Id = group.Id;
        this.Name = group.Name;
        this.Roles = group.RolesManyToMany.Any() ? this.Roles = group.RolesManyToMany.Where(r => r.Role != null).Select(r => new RoleModel(r.Role!)) : this.Roles;
        this.Roles = group.Roles.Any() ? group.Roles.Select(r => new RoleModel(r)) : this.Roles;
    }
    #endregion

    #region Methods
    public Group ToEntity()
    {
        return (Group)this;
    }

    public static explicit operator Group(GroupModel model)
    {
        var group = new Group(model.Name, model.Key)
        {
            Id = model.Id,
            Description = model.Description,
            SortOrder = model.SortOrder,
            IsEnabled = model.IsEnabled,
            Version = model.Version,
        };
        group.RolesManyToMany.AddRange(model.Roles.Select(r => new GroupRole(group.Id, r.Id)));

        return group;
    }
    #endregion
}
