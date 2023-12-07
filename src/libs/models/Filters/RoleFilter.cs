namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class RoleFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public RoleFilter() { }

    public RoleFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.IsEnabled = filter.GetBoolNullValue(nameof(this.IsEnabled));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.Role> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.Role>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.IsEnabled != null)
            predicate = predicate.And((u) => u.IsEnabled == this.IsEnabled);
        return predicate;
    }
    #endregion
}
