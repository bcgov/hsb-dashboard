using HSB.Entities;

namespace HSB.Models;
public class GroupModel : SortableAuditableModel<int>
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<RoleModel> Roles { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<UserModel> Users { get; set; } = [];
    #endregion

    #region Constructors
    public GroupModel() { }

    public GroupModel(Group group) : base(group)
    {
        this.Id = group.Id;
        this.Name = group.Name;
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
