using HSB.Entities;

namespace HSB.Models;

public abstract class SortableAuditableModel<TKey> : CommonAuditableModel<TKey>
    where TKey : notnull
{
    #region Properties
    public int SortOrder { get; set; }
    #endregion

    #region Constructors
    public SortableAuditableModel() { }

    public SortableAuditableModel(SortableAuditable<TKey> entity) : base(entity)
    {
        this.SortOrder = entity.SortOrder;
    }
    #endregion
}
