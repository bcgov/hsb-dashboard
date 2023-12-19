using HSB.Entities;

namespace HSB.Models;

public abstract class CommonAuditableModel<TKey> : AuditableModel
    where TKey : notnull
{
    #region Properties
    public TKey Id { get; set; } = default!;
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
    #endregion

    #region Constructors
    public CommonAuditableModel() { }

    public CommonAuditableModel(CommonAuditable<TKey> entity) : base(entity)
    {
        this.Id = entity.Id;
        this.Name = entity.Name;
        this.Description = entity.Description;
        this.IsEnabled = entity.IsEnabled;
    }
    #endregion
}
