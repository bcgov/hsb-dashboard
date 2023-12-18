namespace HSB.Entities;

/// <summary>
/// DataSync class, provides an entity to map to the database table.
/// </summary>
public class DataSync : CommonAuditable<int>
{
    #region Properties
    /// <summary>
    /// get/set - The service now data type.
    /// </summary>
    public ServiceNowDataType DataType { get; set; }

    /// <summary>
    /// get/set - The offset to start importing data.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// get/set - The query to filter which items will be imported.
    /// </summary>
    public string Query { get; set; } = "";
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
    /// <param name="type"></param>
    /// <param name="query"></param>
    public DataSync(string name, ServiceNowDataType type, string query) : base(name)
    {
        this.DataType = type;
        this.Query = query;
    }
    #endregion
}
