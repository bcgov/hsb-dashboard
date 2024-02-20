namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class ServerItemFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }
    public string? ServiceNowKey { get; set; }
    public int? TenantId { get; set; }
    public int? OrganizationId { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public int? InstallStatus { get; set; }
    public int? NotInstallStatus { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// get/set - Find server items that were updated before this date.
    /// This is used to find server items that did not receive an update in the current sync.
    /// </summary>
    public DateTime? UpdatedBeforeDate { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public ServerItemFilter() { }

    public ServerItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.ServiceNowKey = filter.GetStringValue(nameof(this.ServiceNowKey));
        this.TenantId = filter.GetIntNullValue(nameof(this.TenantId));
        this.OrganizationId = filter.GetIntNullValue(nameof(this.OrganizationId));
        this.OperatingSystemItemId = filter.GetIntNullValue(nameof(this.OperatingSystemItemId));
        this.StartDate = filter.GetDateTimeNullValue(nameof(this.StartDate));
        this.EndDate = filter.GetDateTimeNullValue(nameof(this.EndDate));
        this.UpdatedBeforeDate = filter.GetDateTimeNullValue(nameof(this.UpdatedBeforeDate));
        this.InstallStatus = filter.GetIntNullValue(nameof(this.InstallStatus));
        this.NotInstallStatus = filter.GetIntNullValue(nameof(this.NotInstallStatus));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort), new[] { nameof(ServerItemModel.Name) });
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.ServerItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.ServerItem>();
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
        if (this.InstallStatus != null)
            predicate = predicate.And((u) => u.InstallStatus == this.InstallStatus);
        if (this.NotInstallStatus != null)
            predicate = predicate.And((u) => u.InstallStatus != this.NotInstallStatus);
        if (this.StartDate != null)
            predicate = predicate.And((u) => u.CreatedOn >= this.StartDate.Value.ToUniversalTime());
        if (this.EndDate != null)
            predicate = predicate.And((u) => u.CreatedOn <= this.EndDate.Value.ToUniversalTime());
        if (this.UpdatedBeforeDate != null)
            predicate = predicate.And((u) => u.UpdatedOn < this.UpdatedBeforeDate.Value.ToUniversalTime());

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
