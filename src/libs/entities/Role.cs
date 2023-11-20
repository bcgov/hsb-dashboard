namespace HSB.Entities;

/// <summary>
///
/// </summary>
public class Role : SortableAuditable<int>
{
    #region Properties
    /// <summary>
    ///
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<Group> Groups { get; } = [];

    /// <summary>
    ///
    /// </summary>
    public List<GroupRole> GroupsManyToMany { get; } = [];
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an Role object.
    /// </summary>
    protected Role() { }

    /// <summary>
    /// Creates new instance of a Role object, initializes with specified parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="key"></param>
    public Role(string name, Guid key) : base(name)
    {
        this.Key = key;
    }
    #endregion
}
