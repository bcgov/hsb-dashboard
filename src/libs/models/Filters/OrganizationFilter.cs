namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class OrganizationFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }

    public string? ServiceNowKey { get; set; }

    public int? ParentId { get; set; }

    public int? TenantId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public OrganizationFilter() { }

    public OrganizationFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.IsEnabled = filter.GetBoolNullValue(nameof(this.IsEnabled));
        this.ServiceNowKey = filter.GetStringValue(nameof(this.ServiceNowKey));
        this.ParentId = filter.GetIntNullValue(nameof(this.ParentId));
        this.TenantId = filter.GetIntNullValue(nameof(this.TenantId));
        this.StartDate = filter.GetDateTimeNullValue(nameof(this.StartDate));
        this.EndDate = filter.GetDateTimeNullValue(nameof(this.EndDate));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort), new[] { nameof(OrganizationModel.Name) });
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.Organization> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.Organization>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.IsEnabled != null)
            predicate = predicate.And((u) => u.IsEnabled == this.IsEnabled);
        if (this.ServiceNowKey != null)
            predicate = predicate.And((u) => u.ServiceNowKey == this.ServiceNowKey);
        if (this.ParentId != null)
            predicate = predicate.And((u) => u.ParentId == this.ParentId);
        if (this.TenantId != null)
            predicate = predicate.And((u) => u.TenantsManyToMany.Any(t => t.TenantId == this.TenantId));
        if (this.StartDate != null)
            predicate = predicate.And((u) => u.CreatedOn >= this.StartDate.Value.ToUniversalTime());
        if (this.EndDate != null)
            predicate = predicate.And((u) => u.CreatedOn <= this.EndDate.Value.ToUniversalTime());

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
