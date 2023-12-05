namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class OperatingSystemItemFilter : PageFilter
{
    #region Properties
    public string? UName { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public OperatingSystemItemFilter() { }

    public OperatingSystemItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.UName = filter.GetStringValue(nameof(this.UName));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.OperatingSystemItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.OperatingSystemItem>();
        if (this.UName != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.UName, $"%{this.UName}%"));
        return predicate;
    }
    #endregion
}
