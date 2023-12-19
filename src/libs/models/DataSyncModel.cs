using HSB.Entities;

namespace HSB.Models;
public class DataSyncModel : CommonAuditableModel<int>
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
    /// get/set - The query to filter data.
    /// </summary>
    public string Query { get; set; } = "";
    #endregion

    #region Constructors
    public DataSyncModel() { }

    public DataSyncModel(DataSync entity) : base(entity)
    {
        this.DataType = entity.DataType;
        this.Offset = entity.Offset;
        this.Query = entity.Query;
    }
    #endregion

    #region Methods
    public DataSync ToEntity()
    {
        return (DataSync)this;
    }

    public static explicit operator DataSync(DataSyncModel model)
    {
        var entity = new DataSync(model.Name, model.DataType, model.Query)
        {
            Id = model.Id,
            Description = model.Description,
            Offset = model.Offset,
            IsEnabled = model.IsEnabled,
            Version = model.Version,
        };

        return entity;
    }
    #endregion
}
