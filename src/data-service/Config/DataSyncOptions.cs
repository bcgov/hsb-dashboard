using HSB.Entities;

namespace HSB.DataService;

/// <summary>
/// DataSyncOptions class, provides configuration settings for data sync items.
/// </summary>
public class DataSyncOptions
{
    #region Properties
    /// <summary>
    /// get/set - Name of data sync.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// get/set - The service now data type.
    /// </summary>
    public ServiceNowDataType DataType { get; set; }

    /// <summary>
    /// get/set - The start offset when fetching items. (-1 to turn off)
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// get/set - Query statement to filter data items.
    /// </summary>
    public string Query { get; set; } = "";

    /// <summary>
    /// get/set - Model to sync with the database.
    /// </summary>
    public Models.DataSyncModel? Model { get; set; }
    #endregion
}
