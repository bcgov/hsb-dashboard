namespace HSB.Entities;

/// <summary>
/// Auditable abstract class, provides common audit columns.
/// </summary>
public abstract class Auditable
{
    #region Properties
    /// <summary>
    /// get/set - When the record was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// get/set - The name to identify the user who created the record.
    /// </summary>
    public string CreatedBy { get; set; } = "";

    /// <summary>
    /// get/set - When the records was last updated.
    /// </summary>
    public DateTimeOffset UpdatedOn { get; set; }

    /// <summary>
    /// get/set - The name to identity the user who last updated the record.
    /// </summary>
    public string UpdatedBy { get; set; } = "";

    /// <summary>
    /// get/set - The record version number for optimistic concurrency.
    /// </summary>
    public long Version { get; set; }
    #endregion
}
