namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class Group : SortableAuditable<int>
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<Role> Roles { get; } = new List<Role>();

    /// <summary>
    ///
    /// </summary>
    public List<GroupRole> RolesManyToMany { get; } = new List<GroupRole>();

    /// <summary>
    ///
    /// </summary>
    public List<User> Users { get; } = new List<User>();

    /// <summary>
    ///
    /// </summary>
    public List<UserGroup> UsersManyToMany { get; } = new List<UserGroup>();
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an Group object.
    /// </summary>
    protected Group() { }

    /// <summary>
    /// Creates new instance of a Group object, initializes with specified parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="key"></param>
    public Group(string name, Guid key) : base(name)
    {
        this.Key = key;
    }
    #endregion
}
