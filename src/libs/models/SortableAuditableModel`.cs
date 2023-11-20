using HSB.Entities;

namespace HSB.Models;

public abstract class SortableAuditableModel<TKey> : AuditableModel
    where TKey : notnull
{
    #region Properties
    public TKey Id { get; set; } = default!;
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int SortOrder { get; set; }
    public bool IsEnabled { get; set; } = true;
    #endregion

    #region Constructors
    public SortableAuditableModel() { }

    public SortableAuditableModel(SortableAuditable<TKey> entity) : base(entity)
    {
        this.Id = entity.Id;
        this.Name = entity.Name;
        this.Description = entity.Description;
        this.SortOrder = entity.SortOrder;
        this.IsEnabled = entity.IsEnabled;
    }
    #endregion
}
