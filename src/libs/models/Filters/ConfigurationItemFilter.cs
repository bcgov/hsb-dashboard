namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class ConfigurationItemFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }

    public int? OrganizationId { get; set; }

    public int? TenantId { get; set; }

    public string? ClassName { get; set; }

    public string? ServiceNowKey { get; set; }

    public string? Category { get; set; }

    public string? SubCategory { get; set; }

    public string? Platform { get; set; }

    public string? DnsDomain { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public ConfigurationItemFilter() { }

    public ConfigurationItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.OrganizationId = filter.GetIntNullValue(nameof(this.OrganizationId));
        this.TenantId = filter.GetIntNullValue(nameof(this.TenantId));
        this.ClassName = filter.GetStringValue(nameof(this.ClassName));
        this.ServiceNowKey = filter.GetStringValue(nameof(this.ServiceNowKey));
        this.Category = filter.GetStringValue(nameof(this.Category));
        this.SubCategory = filter.GetStringValue(nameof(this.SubCategory));
        this.Platform = filter.GetStringValue(nameof(this.Platform));
        this.DnsDomain = filter.GetStringValue(nameof(this.DnsDomain));
        this.StartDate = filter.GetDateTimeNullValue(nameof(this.StartDate));
        this.EndDate = filter.GetDateTimeNullValue(nameof(this.EndDate));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort), new[] { nameof(ConfigurationItemModel.Name) });
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.ConfigurationItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.ConfigurationItem>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.OrganizationId.HasValue)
            predicate = predicate.And((u) => u.OrganizationId == this.OrganizationId.Value);
        if (this.TenantId.HasValue)
            predicate = predicate.And((u) => u.TenantId == this.TenantId.Value);
        if (this.ClassName != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ClassName, this.ClassName));
        if (this.ServiceNowKey != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ServiceNowKey, this.ServiceNowKey));
        if (this.Category != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Category, this.Category));
        if (this.SubCategory != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.SubCategory, this.SubCategory));
        if (this.Platform != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Platform, this.Platform));
        if (this.DnsDomain != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.DnsDomain, this.DnsDomain));
        if (this.StartDate != null)
            predicate = predicate.And((u) => u.CreatedOn >= this.StartDate);
        if (this.EndDate != null)
            predicate = predicate.And((u) => u.CreatedOn <= this.EndDate);

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
