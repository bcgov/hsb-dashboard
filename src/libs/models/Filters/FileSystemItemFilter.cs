namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class FileSystemItemFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }
    public string? ServiceNowKey { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? ClassName { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public FileSystemItemFilter() { }

    public FileSystemItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.ClassName = filter.GetStringValue(nameof(this.ClassName));
        this.ServiceNowKey = filter.GetStringValue(nameof(this.ServiceNowKey));
        this.Category = filter.GetStringValue(nameof(this.Category));
        this.SubCategory = filter.GetStringValue(nameof(this.SubCategory));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.FileSystemItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.FileSystemItem>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.ClassName != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ClassName, this.ClassName));
        if (this.ServiceNowKey != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ServiceNowKey, this.ServiceNowKey));
        if (this.Category != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Category, this.Category));
        if (this.SubCategory != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.SubCategory, this.SubCategory));

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
