using HSB.Entities;

namespace HSB.Models;

public abstract class AuditableModel
{
    #region Properties
    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; } = "";
    public DateTimeOffset UpdatedOn { get; set; }
    public string UpdatedBy { get; set; } = "";
    public long Version { get; set; }
    #endregion

    #region Constructors
    public AuditableModel() { }

    public AuditableModel(Auditable entity)
    {
        this.CreatedBy = entity.CreatedBy;
        this.CreatedOn = entity.CreatedOn;
        this.UpdatedBy = entity.UpdatedBy;
        this.UpdatedOn = entity.UpdatedOn;
        this.Version = entity.Version;
    }
    #endregion
}
