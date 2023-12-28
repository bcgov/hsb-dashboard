namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class UserGroup : Auditable
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
    public int GroupId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Group? Group { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="group"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserGroup(User user, Group group)
    {
        this.User = user ?? throw new ArgumentNullException(nameof(user));
        this.UserId = user.Id;
        this.Group = group ?? throw new ArgumentNullException(nameof(group));
        this.GroupId = group.Id;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    public UserGroup(int userId, int groupId)
    {
        this.UserId = userId;
        this.GroupId = groupId;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not UserGroup entity) return false;
        return (this.UserId, this.GroupId).Equals((entity.UserId, entity.GroupId));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, GroupId);
    }
    #endregion
}
