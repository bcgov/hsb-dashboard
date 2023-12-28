namespace HSB.Entities;

/// <summary>
/// DataSync class, provides an entity to map to the database table.
/// </summary>
public class DataSync : SortableAuditable<int>
{
    #region Properties
    /// <summary>
    /// get/set - The offset to start importing data.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// get/set - The query to filter which items will be imported.
    /// </summary>
    public string Query { get; set; } = "";

    /// <summary>
    /// get/set - Whether this data sync is currently active.
    /// A failed data sync will remain active until it is completed.
    /// </summary>
    public bool IsActive { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an DataSync object.
    /// </summary>
    protected DataSync() { }

    /// <summary>
    /// Creates new instance of a DataSync object, initializes with specified parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="query"></param>
    public DataSync(string name, string query) : base(name)
    {
        this.Query = query;
    }
    #endregion
}
