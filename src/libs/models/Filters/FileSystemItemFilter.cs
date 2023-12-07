namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class FileSystemItemFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public FileSystemItemFilter() { }

    public FileSystemItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.FileSystemItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.FileSystemItem>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        return predicate;
    }
    #endregion
}
