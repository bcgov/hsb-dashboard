using HSB.Entities;

namespace HSB.Models;

public abstract class SortableCodeAuditableModel<TKey> : SortableAuditableModel<TKey>
    where TKey : notnull
{
    #region Properties
    public string Code { get; set; } = "";
    #endregion

    #region Constructors
    public SortableCodeAuditableModel() { }

    public SortableCodeAuditableModel(SortableCodeAuditable<TKey> entity) : base(entity)
    {
        this.Code = entity.Code;
    }
    #endregion
}
