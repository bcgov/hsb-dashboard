namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class TenantFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public TenantFilter() { }

    public TenantFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.IsEnabled = filter.GetBoolNullValue(nameof(this.IsEnabled));
        this.StartDate = filter.GetDateTimeNullValue(nameof(this.StartDate));
        this.EndDate = filter.GetDateTimeNullValue(nameof(this.EndDate));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort), new[] { nameof(TenantModel.Name) });
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.Tenant> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.Tenant>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.IsEnabled != null)
            predicate = predicate.And((u) => u.IsEnabled == this.IsEnabled);
        if (this.StartDate != null)
            predicate = predicate.And((u) => u.CreatedOn >= this.StartDate.Value.ToUniversalTime());
        if (this.EndDate != null)
            predicate = predicate.And((u) => u.CreatedOn <= this.EndDate.Value.ToUniversalTime());

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
