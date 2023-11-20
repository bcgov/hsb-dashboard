namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class GroupRole : Auditable
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Group? Group { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Role? Role { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    ///
    /// </summary>
    protected GroupRole()
    {

    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="group"></param>
    /// <param name="role"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GroupRole(Group group, Role role)
    {
        this.Group = group ?? throw new ArgumentNullException(nameof(group));
        this.GroupId = group.Id;
        this.Role = role ?? throw new ArgumentNullException(nameof(role));
        this.RoleId = role.Id;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleId"></param>
    public GroupRole(int groupId, int roleId)
    {
        this.GroupId = groupId;
        this.RoleId = roleId;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not GroupRole entity) return false;
        return (this.GroupId, this.RoleId).Equals((entity.GroupId, entity.RoleId));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(GroupId, RoleId);
    }
    #endregion
}
