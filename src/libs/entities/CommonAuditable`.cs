namespace HSB.Entities;

/// <summary>
/// CommonAuditable abstract class, provides common columns for tables.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class CommonAuditable<TKey> : Auditable
  where TKey : notnull
{
    #region Properties
    /// <summary>
    /// get/set - The primary key.
    /// </summary>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// get/set - A unique name to identify this record.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// get/set - A description of this record.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// get/set - Whether this record is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an CommonAuditable object.
    /// </summary>
    protected CommonAuditable() { }

    /// <summary>
    /// Creates a new instance of a CommonAuditable object, initializes with specified parameters.
    /// </summary>
    /// <param name="name"></param>
    public CommonAuditable(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Creates a new instance of a CommonAuditable object, initializes with specified parameters.
    /// </summary>
    /// <param name="entity"></param>
    public CommonAuditable(CommonAuditable<TKey> entity) : base(entity)
    {
        this.Id = entity.Id;
        this.Name = entity.Name;
        this.Description = entity.Description;
        this.IsEnabled = entity.IsEnabled;
    }
    #endregion
}
