namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class ServerHistoryItemFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }
    public string? ServiceNowKey { get; set; }
    public int? TenantId { get; set; }
    public int? OrganizationId { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public ServerHistoryItemFilter() { }

    public ServerHistoryItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.ServiceNowKey = filter.GetStringValue(nameof(this.ServiceNowKey));
        this.TenantId = filter.GetIntNullValue(nameof(this.TenantId));
        this.OrganizationId = filter.GetIntNullValue(nameof(this.OrganizationId));
        this.OperatingSystemItemId = filter.GetIntNullValue(nameof(this.OperatingSystemItemId));
        this.StartDate = filter.GetDateTimeNullValue(nameof(this.StartDate));
        this.EndDate = filter.GetDateTimeNullValue(nameof(this.EndDate));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort), new[] { nameof(ServerHistoryItemModel.Name) });
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.ServerHistoryItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.ServerHistoryItem>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.ServiceNowKey != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ServiceNowKey, this.ServiceNowKey));
        if (this.TenantId != null)
            predicate = predicate.And((u) => u.TenantId == this.TenantId);
        if (this.OrganizationId != null)
            predicate = predicate.And((u) => u.OrganizationId == this.OrganizationId);
        if (this.OperatingSystemItemId != null)
            predicate = predicate.And((u) => u.OperatingSystemItemId == this.OperatingSystemItemId);
        if (this.StartDate != null)
            predicate = predicate.And((u) => u.CreatedOn >= this.StartDate.Value.ToUniversalTime());
        if (this.EndDate != null)
            predicate = predicate.And((u) => u.CreatedOn <= this.EndDate.Value.ToUniversalTime());

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
