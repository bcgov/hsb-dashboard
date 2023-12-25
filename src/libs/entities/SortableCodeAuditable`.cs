namespace HSB.Entities;

/// <summary>
/// SortableCodeAuditable abstract class, provides common sortable columns.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class SortableCodeAuditable<TKey> : SortableAuditable<TKey>
  where TKey : notnull
{
    #region Properties
    /// <summary>
    /// get/set - A unique code to identify this record.
    /// </summary>
    public string Code { get; set; } = "";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an SortableCodeAuditable object.
    /// </summary>
    protected SortableCodeAuditable() { }

    /// <summary>
    /// Creates new instance of a SortableCodeAuditable object, initializes with specified parameters.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="name"></param>
    public SortableCodeAuditable(string code, string name) : base(name)
    {
        this.Code = code;
    }

    /// <summary>
    /// Creates new instance of a SortableCodeAuditable object, initializes with specified parameters.
    /// </summary>
    /// <param name="entity"></param>
    public SortableCodeAuditable(SortableCodeAuditable<TKey> entity) : base(entity)
    {
        this.Code = entity.Code;
    }
    #endregion
}
