using HSB.Entities;

namespace HSB.Models;
public class DataSyncModel : SortableAuditable<int>
{
    #region Properties
    /// <summary>
    /// get/set - The offset to start importing data.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// get/set - The query to filter data.
    /// </summary>
    public string Query { get; set; } = "";

    /// <summary>
    /// get/set - Whether this data sync is active.
    /// </summary>
    public bool IsActive { get; set; }
    #endregion

    #region Constructors
    public DataSyncModel() { }

    public DataSyncModel(DataSync entity) : base(entity)
    {
        this.Offset = entity.Offset;
        this.Query = entity.Query;
        this.IsActive = entity.IsActive;
    }
    #endregion

    #region Methods
    public DataSync ToEntity()
    {
        return (DataSync)this;
    }

    public static explicit operator DataSync(DataSyncModel model)
    {
        var entity = new DataSync(model.Name, model.Query)
        {
            Id = model.Id,
            Description = model.Description,
            Offset = model.Offset,
            IsActive = model.IsActive,
            IsEnabled = model.IsEnabled,
            Version = model.Version,
        };

        return entity;
    }
    #endregion
}
